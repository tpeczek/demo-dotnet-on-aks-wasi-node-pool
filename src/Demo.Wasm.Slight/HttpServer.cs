using System;
using System.Runtime.InteropServices;
using Demo.Wasm.Slight.Wasi;

namespace Demo.Wasm.Slight
{
    internal static class HttpServer
    {
        // Any library name that P/Invoke generator knows
        private const string LIBRARY_NAME = "libSystem.Native";

        private static uint? _index;

        public static void Serve(string address)
        {
            if (_index.HasValue)
            {
                throw new Exception("The server is already running!");
            }

            HttpRouter router = HttpRouter.Create();
            http_server_serve(WasiString.FromString(address), router.Index, out WasiExpected<uint> expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

            _index = expected.Result;
        }

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_server_serve(WasiString address, uint httpRouterIndex, out WasiExpected<uint> ret0);

        [DllImport(LIBRARY_NAME)]
        internal static extern unsafe void http_server_stop(uint httpServerIndex, out WasiExpected<uint> ret0);
    }
}