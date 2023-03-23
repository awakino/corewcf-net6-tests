
// we have to do it this way with a Startup class because the older Host.CreateDefaultBuilder
// method for starting a service doesn't support minimal APIs. And we have to do that because
// WebApplicationFactory won't work with WCF - the ChannelFactory makes actual network requests.
var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder =>
    {
        // use ConfigureKestrel here insead of UseKestrel. This will work both locally
        // and in Azure because ConfigureWebHostDefaults will figure it out. UseKestrel
        // overrides that and won't work in Azure. ConfigureKestrel will setup Kestrel 
        // properly locally, and do nothing when Kestrel isn't being used.
        builder.ConfigureKestrel(options =>
        {
            options.AllowSynchronousIO = true;
        });

        builder.UseStartup<Startup>();
    }).Build();

host.Run();
