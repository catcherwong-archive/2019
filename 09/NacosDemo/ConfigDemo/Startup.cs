namespace ConfigDemo
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Nacos;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // configuration
            services.AddNacos(configure =>
            {
                // default timeout
                configure.DefaultTimeOut = 8;
                // nacos's endpoint
                configure.ServerAddresses = new System.Collections.Generic.List<string> { "localhost:8848" };
                // namespace
                configure.Namespace = "";
                // listen interval
                configure.ListenInterval = 1000;
            });
            services.AddHostedService<ListenConfigurationBgTask>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
