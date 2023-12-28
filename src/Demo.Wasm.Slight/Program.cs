using Demo.Wasm.Slight;

Console.WriteLine("-- Demo.Wasm.Slight --");

HttpRouter router = HttpRouter.Create();
router = router
    .Get("/hello", (HttpRequest request) =>
    {
        HttpResponse response = new HttpResponse(200);
        response.SetHeaders(new[] { KeyValuePair.Create("Content-Type", "text/plain") });
        response.SetBody($"Hello from Demo.Wasm.Slight!");

        return response;
    })
    .Get("/goodbye", (HttpRequest request) =>
    {
        HttpResponse response = new HttpResponse(200);
        response.SetHeaders(new[] { KeyValuePair.Create("Content-Type", "text/plain") });
        response.SetBody($"Goodbye from Demo.Wasm.Slight!");

        return response;
    });

Console.WriteLine("Demo.Wasm.Slight: Starting server");

HttpServer.Serve("0.0.0.0:80", router);

Console.WriteLine("Demo.Wasm.Slight: Moving on");