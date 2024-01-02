using System.Runtime.InteropServices;
using Demo.Wasm.Slight.Wasi;

namespace Demo.Wasm.Slight
{
    internal class HttpRouter
    {
        // Any library name that P/Invoke generator knows
        private const string _libraryName = "libSystem.Native";

        [DllImport(_libraryName)]
        internal static extern unsafe void http_router_new(out WasiExpected<uint> ret0);

        [DllImport(_libraryName)]
        internal static extern unsafe void http_router_get(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);

        [DllImport(_libraryName)]
        internal static extern unsafe void http_router_put(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);

        [DllImport(_libraryName)]
        internal static extern unsafe void http_router_post(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);

        [DllImport(_libraryName)]
        internal static extern unsafe void http_router_delete(uint httpRouterIndex, WasiString route, WasiString handler, out WasiExpected<uint> ret0);
    }
}