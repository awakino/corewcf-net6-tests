using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Sample.CoreWCF.Test
{
    public class ServiceFixture : IDisposable
    {
        private readonly IHost _server;

        public Uri ServerUri { get; }

        private readonly IMessageSink _diagnostics;

        public ServiceFixture(IMessageSink diagnostics)
        {
            _diagnostics = diagnostics;
            _diagnostics.OnMessage(new DiagnosticMessage("Starting SOAP service..."));

            // NOTE: At the moment the test port MUST be the same as the one the actual service will
            // listen on. I'm not sure why but it looks like the fixture is still picking up the ports
            // specified in the appsettings.json for the service project.
            ServerUri = new Uri("https://localhost:5001");

            // start the API service - note that this cannot use WebApplicationFactory as the WCF
            // ChannelFactory cannot communicate with an in-memory service so we have to use a 'full'
            // service that actually listens for network requests
            _server = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults((builder) =>
                {
                    builder.UseUrls(ServerUri.ToString());
                    builder.UseStartup<Startup>();
                }).Build();

            _server.Start();
        }

        public void Dispose()
        {
            _server.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
