using System.Runtime.InteropServices;
using Demo.Wasm.Slight.Wasi;

namespace Demo.Wasm.Slight
{
    internal static class HttpServer
    {
        // Any library name that P/Invoke generator knows
        private const string _libraryName = "libSystem.Native";

        [DllImport(_libraryName)]
        internal static extern unsafe void http_server_serve(WasiString address, uint httpRouterIndex, out WasiExpected<uint> ret0);

        [DllImport(_libraryName)]
        internal static extern unsafe void http_server_stop(uint httpServerIndex, out WasiExpected<uint> ret0);
    }
}