using Demo.Wasm.Slight.Wasi;
using System.Runtime.InteropServices;

namespace Demo.Wasm.Slight
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct HttpRouter
    {
        private readonly uint _idx;

        public readonly uint Index => _idx;

        public static HttpRouter Create()
        {
            WasiExpected<HttpRouter> expected = new WasiExpected<HttpRouter>();

            HttpRouterFunctions.New(ref expected);

            if (expected.IsError)
            {
                throw new Exception(expected.Error?.ErrorWithDescription.ToString());
            }

#pragma warning disable CS8629 // Nullable value type may be null.
            return (HttpRouter)expected.Result;
#pragma warning restore CS8629 // Nullable value type may be null.
        }
    }
}
