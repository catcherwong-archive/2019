namespace AService
{
    using Jaeger;
    using Jaeger.Reporters;
    using Jaeger.Samplers;
    using Jaeger.Senders;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using OpenTracing;
    using OpenTracing.Util;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient();

            services.AddOpenTracing(new System.Collections.Generic.Dictionary<string,LogLevel>
            {
                {"AService", LogLevel.Information}
            });

            // Adds the Jaeger Tracer.
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                string serviceName = serviceProvider.GetRequiredService<IHostingEnvironment>().ApplicationName;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var sampler = new ConstSampler(sample: true);
                var reporter = new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory) // optional, defaults to no logging
                                                          //.WithMaxQueueSize(...)            // optional, defaults to 100
                                                          //.WithFlushInterval(...)           // optional, defaults to TimeSpan.FromSeconds(1)
                        .WithSender(new UdpSender("jagerservice", 6831, 0))                  // optional, defaults to UdpSender("localhost", 6831, 0)
                        .Build();

                // This will log to a default localhost installation of Jaeger.
                var tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .WithReporter(reporter)
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
