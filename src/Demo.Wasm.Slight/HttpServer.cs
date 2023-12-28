﻿using Demo.Wasm.Slight.Wasi;
using System;

namespace Demo.Wasm.Slight
{
    internal static class HttpServer
    {
        private static uint? _index;
        private static HttpRouter? _router;

        public static void Serve(string address, HttpRouter router)
        {
            if (_index.HasValue || (_router is not null))
            {
                throw new Exception("The server is already running!");
            }

            WasiString wasiAddress = WasiString.FromString(address);
            WasiExpected<uint> expected = new WasiExpected<uint>();

            HttpServerFunctions.Serve(ref wasiAddress, router.Index, ref expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _index = expected.Result;
            _router = router;
        }

        public static void Stop()
        {
            WasiExpected<uint> expected = new WasiExpected<uint>();

            if (_index.HasValue)
            {
                HttpServerFunctions.Stop(_index.Value, ref expected);

                _index = null;
                _router = null;

                if (expected.IsError)
                {
                    throw new Exception(expected.Error?.ErrorWithDescription.ToString());
                }
            }
        }

        private static unsafe void HandleRequest(ref HttpRequest request, out WasiExpected<HttpResponse> result)
        {
            Func<HttpRequest, HttpResponse>? handler = _router?.GetHandler(request.Method, request.Uri);
            if (handler is null)
            {

                HttpResponse response = new HttpResponse(404);
                response.SetBody($"Handler Not Found (Requested URI: {request.Uri})");

                result = new WasiExpected<HttpResponse>(response);
            }
            else
            {
                result = new(handler.Invoke(request));
            }
        }
    }
}
