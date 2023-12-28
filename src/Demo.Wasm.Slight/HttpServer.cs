using Demo.Wasm.Slight.Wasi;
using System.Runtime.InteropServices;

namespace Demo.Wasm.Slight
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct HttpServer
    {
        private readonly uint _idx;

        public readonly uint Index => _idx;

        public static HttpServer Serve(string address, HttpRouter router)
        {
            WasiString wasiAddress = WasiString.FromString(address);
            WasiExpected<HttpServer> expected = new WasiExpected<HttpServer>();

            HttpServerFunctions.Serve(ref wasiAddress, router, ref expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

#pragma warning disable CS8629 // Nullable value type may be null.
            return (HttpServer)expected.Result;
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        public void Stop()
        {
            WasiExpected<HttpServer> expected = new WasiExpected<HttpServer>();

            HttpServerFunctions.Stop(this, ref expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }
        }
    }
}
