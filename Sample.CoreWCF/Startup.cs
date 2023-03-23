namespace Sample.CoreWCF
{
    public class Startup
    {
        /// <summary>
        ///     Application Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Startup class constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web Host Environment</param>
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseServiceModel(serviceBuilder =>
            {
                serviceBuilder.AddService<Service>(options =>
                {
                    // enable the inclusion of the actual exception detail in service faults
                    options.DebugBehavior.IncludeExceptionDetailInFaults = env.IsDevelopment();
                });

                serviceBuilder.AddServiceEndpoint<Service, IService>(
                    new BasicHttpBinding(BasicHttpSecurityMode.Transport),
                    "TestService/Service.svc");

                //serviceBuilder.AddServiceEndpoint<Service, IService>(
                //    new BasicHttpBinding(BasicHttpSecurityMode.None),
                //    "TestService/InsecureService.svc");

                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpsGetEnabled = true;
                //serviceMetadataBehavior.HttpGetEnabled = true;
            });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices();
            services.AddServiceModelMetadata();
            services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

            services.AddApplicationInsightsTelemetry();
        }
    }
}
