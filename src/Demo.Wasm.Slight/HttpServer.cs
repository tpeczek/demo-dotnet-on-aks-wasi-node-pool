using Demo.Wasm.Slight.Wasi;

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

            HttpServerFunctions.Serve(WasiString.FromString(address), router.Index, out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _index = expected.Result;
            _router = router;
        }

        public static void Stop()
        {
            if (_index.HasValue)
            {
                HttpServerFunctions.Stop(_index.Value, out WasiExpected<uint> expected);

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
            string route = request.Uri.AbsolutePath;
            Func<HttpRequest, HttpResponse>? handler = _router?.GetHandler(request.Method, route);
            if (handler is null)
            {

                HttpResponse response = new HttpResponse(404);
                response.SetBody($"Handler Not Found (Requested route: {route})");

                result = new WasiExpected<HttpResponse>(response);
            }
            else
            {
                result = new(handler.Invoke(request));
            }
        }
    }
}
