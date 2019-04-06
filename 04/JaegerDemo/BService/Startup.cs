namespace BService
{
    using Jaeger;
    using Jaeger.Samplers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OpenTracing;
    using OpenTracing.Util;
    using EasyCaching.Core;
    using EasyCaching.InMemory;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Jaeger.Senders;
    using Jaeger.Reporters;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            // Adds an InMemory-Sqlite DB to show EFCore traces.
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<BDbContext>(options =>
                {
                    var connectionStringBuilder = new SqliteConnectionStringBuilder
                    {
                        DataSource = ":memory:",
                        Mode = SqliteOpenMode.Memory,
                        Cache = SqliteCacheMode.Shared
                    };
                    var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);

                    connection.Open();
                    connection.EnableExtensions(true);

                    options.UseSqlite(connection);
                });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddEasyCaching(options =>
            {
                options.UseInMemory("m1");
            });

            services.AddOpenTracing();

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

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BDbContext>();
                dbContext.Seed();
            }

            app.UseMvc();
        }
    }
}
