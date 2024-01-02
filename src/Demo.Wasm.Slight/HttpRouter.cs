using System;
using System.Runtime.InteropServices;
using Demo.Wasm.Slight.Wasi;

namespace Demo.Wasm.Slight
{
    internal class HttpRouter
    {
        // Any library name that P/Invoke generator knows
        private const string LIBRARY_NAME = "libSystem.Native";

        private uint _index;

        public uint Index => _index;

        private HttpRouter(uint index)
        {
            _index = index;
        }

        public static HttpRouter Create()
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