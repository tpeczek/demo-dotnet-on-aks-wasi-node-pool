using Demo.Wasm.Slight.Wasi;
using System.Runtime.InteropServices;

namespace Demo.Wasm.Slight
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HttpResponse
    {
        private readonly ushort _status;
        private WasiOption<WasiStringKeyValuePairList> _headers;
        private WasiOption<WasiBuffer> _body;

        public HttpResponse(int status)
        {
            _status = (ushort)status;
        }

        public void SetHeaders(IReadOnlyCollection<KeyValuePair<string, string>> headers)
        {
            _headers = new WasiOption<WasiStringKeyValuePairList>(WasiStringKeyValuePairList.FromEnumerable(headers));
        }

        public void SetBody(string body)
        {
            _body = new WasiOption<WasiBuffer>(WasiBuffer.FromString(body));
        }

        
    }
}
