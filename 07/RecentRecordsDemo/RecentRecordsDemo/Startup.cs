namespace RecentRecordsDemo
{
    using EasyCaching.Core;
    using EasyCaching.CSRedis;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEasyCaching(options => 
            {
                options.UseCSRedis(x => 
                {
                    x.DBConfig = new CSRedisDBOptions
                    {
                        ConnectionStrings = new List<string> { "127.0.0.1:6379" }
                    };
                    x.EnableLogging = false;
                    x.MaxRdSecond = 0;
                }, "redis");
            });
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
