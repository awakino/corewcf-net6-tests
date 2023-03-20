
// we have to do it this way with a Startup class because the older Host.CreateDefaultBuilder
// method for starting a service doesn't support minimal APIs. And we have to do that because
// WebApplicationFactory won't work with WCF - the ChannelFactory makes actual network requests.
var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder =>
    {
        builder.UseKestrel();
        builder.UseStartup<Startup>();
    }).Build();

host.Run();
