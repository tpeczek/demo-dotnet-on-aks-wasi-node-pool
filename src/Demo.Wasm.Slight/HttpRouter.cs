using Demo.Wasm.Slight.Wasi;

namespace Demo.Wasm.Slight
{
    internal class HttpRouter
    {
        private static readonly WasiString REQUEST_HANDLER = WasiString.FromString("handle-http");

        private readonly uint _index;
        private readonly Dictionary<string, Func<HttpRequest, HttpResponse>> _getHandlers = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
        private readonly Dictionary<string, Func<HttpRequest, HttpResponse>> _putHandlers = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
        private readonly Dictionary<string, Func<HttpRequest, HttpResponse>> _postHandlers = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
        private readonly Dictionary<string, Func<HttpRequest, HttpResponse>> _deleteHandlers = new Dictionary<string, Func<HttpRequest, HttpResponse>>();

        public uint Index => _index;

        private HttpRouter(uint index)
        {
            _index = index;
        }

        public static HttpRouter Create()
        {
            HttpRouterFunctions.New(out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

#pragma warning disable CS8629 // Nullable value type may be null.
            return new HttpRouter(expected.Result.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        public HttpRouter Get(string route, Func<HttpRequest, HttpResponse> handler)
        {
            if (_getHandlers.ContainsKey(route))
            {
                throw new Exception($"{route} is already registered.");
            }

            HttpRouterFunctions.Get(_index, WasiString.FromString(route), REQUEST_HANDLER, out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _getHandlers.Add(route, handler);

            return this;
        }

        public HttpRouter Put(string route, Func<HttpRequest, HttpResponse> handler)
        {
            if (_putHandlers.ContainsKey(route))
            {
                throw new Exception($"{route} is already registered.");
            }

            HttpRouterFunctions.Put(_index, WasiString.FromString(route), REQUEST_HANDLER, out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _putHandlers.Add(route, handler);

            return this;
        }

        public HttpRouter Post(string route, Func<HttpRequest, HttpResponse> handler)
        {
            if (_postHandlers.ContainsKey(route))
            {
                throw new Exception($"{route} is already registered.");
            }

            HttpRouterFunctions.Post(_index, WasiString.FromString(route), REQUEST_HANDLER, out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _postHandlers.Add(route, handler);

            return this;
        }

        public HttpRouter Delete(string route, Func<HttpRequest, HttpResponse> handler)
        {
            if (_deleteHandlers.ContainsKey(route))
            {
                throw new Exception($"{route} is already registered.");
            }

            HttpRouterFunctions.Delete(_index, WasiString.FromString(route), REQUEST_HANDLER, out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _deleteHandlers.Add(route, handler);

            return this;
        }

        internal Func<HttpRequest, HttpResponse>? GetHandler(HttpMethod method, string uri)
        {
            switch (method)
            {
                case HttpMethod.GET:
                    return _getHandlers.GetValueOrDefault(uri);
                case HttpMethod.PUT:
                    return _putHandlers.GetValueOrDefault(uri);
                case HttpMethod.POST:
                    return _postHandlers.GetValueOrDefault(uri);
                case HttpMethod.DELETE:
                    return _deleteHandlers.GetValueOrDefault(uri);
                default:
                    return null;
            }
        }
    }
}
