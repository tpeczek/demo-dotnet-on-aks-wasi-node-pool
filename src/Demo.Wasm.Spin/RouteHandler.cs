using System.Net;
using Fermyon.Spin.Sdk;

namespace Demo.Wasm.Spin;

public static class RouteHandler
{
    [HttpHandler]
    public static HttpResponse HandleHttpRequest(HttpRequest request)
    {
        return new HttpResponse
        {
            StatusCode = HttpStatusCode.OK,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" },
            },
            BodyAsString = "-- Demo.Wasm.Spin --"
        };
    }
}
