using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Demo.Wasm.Slight.Wasi;

namespace Demo.Wasm.Slight
{
    internal class HttpRouter
    {
        // Any library name that P/Invoke generator knows
        private const string LIBRARY_NAME = "libSystem.Native";
        private static readonly WasiString REQUEST_HANDLER = WasiString.FromString("handle-http");
        private static readonly Type HTTP_HANDLER_ATTRIBUTE_TYPE = typeof(HttpHandlerAttribute);

        private static object _routesLock = new Object();
        private static Dictionary<HttpHandlerAttribute, MethodInfo>? _routes;

        private uint _index;

        internal uint Index => _index;

        private HttpRouter(uint index)
        {
            _index = index;
        }

        internal static HttpRouter Create()
        {
            http_router_new(out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

#pragma warning disable CS8629 // Nullable value type may be null.
            return new HttpRouter(expected.Result.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        internal HttpRouter RegisterRoutes()
        {
            DiscoverRoutes();

            HttpRouter router = this;
            foreach (KeyValuePair<HttpHandlerAttribute, MethodInfo> route in _routes)
            {
                router = router.RegisterRoute(route.Key.Method, route.Key.Route);
            }

            return router;
        }

        internal static HttpResponse? InvokeRouteHandler(HttpRequest request)
        {
            DiscoverRoutes();

            HttpHandlerAttribute attribute = new HttpHandlerAttribute(request.Method, request.Uri.AbsolutePath);
            MethodInfo handler = _routes.GetValueOrDefault(attribute);

            return (handler is null) ? null : (HttpResponse)handler.Invoke(null, new object[] { request });
        }

        private static void DiscoverRoutes()
        {
            if (_routes is null)
            {
                lock (_routesLock)
                {
                    if (_routes is null)
                    {
                        _routes = new Dictionary<HttpHandlerAttribute, MethodInfo>();

                        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                        {
                            foreach(MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                            {
                                foreach (object attribute in method.GetCustomAttributes(HTTP_HANDLER_ATTRIBUTE_TYPE, false))
                                {
                                    _routes.Add((HttpHandlerAttribute)attribute, method);
                                }
                            }
                        }
                    }
                }
            }
        }

        private HttpRouter RegisterRoute(HttpMethod method, string route)
        {
            WasiExpected<uint> expected;

            switch (method)
            {
                case HttpMethod.GET:
                    http_router_get(_index, WasiString.FromString(route), REQUEST_HANDLER, out expected);
                    break;
                case HttpMethod.PUT:
                    http_router_put(_index, WasiString.FromString(route), REQUEST_HANDLER, out expected);
                    break;
                case HttpMethod.POST:
                    http_router_post(_index, WasiString.FromString(route), REQUEST_HANDLER, out expected);
                    break;
                case HttpMethod.DELETE:
                    http_router_delete(_index, WasiString.FromString(route), REQUEST_HANDLER, out expected);
                    break;
                default:
                    throw new NotSupportedException($"The HTTP request method {method} is not supported.");
            }

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

#pragma warning disable CS8629 // Nullable value type may be null.
            return new HttpRouter(expected.Result.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_router_new(out WasiExpected<uint> ret0);

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_router_get(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_router_put(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_router_post(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_router_delete(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);
    }
}