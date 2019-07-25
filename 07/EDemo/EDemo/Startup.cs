using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCaching.Core;
using EasyCaching.CSRedis;
using EasyCaching.InMemory;
using EasyCaching.Serialization.MessagePack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //configuration
            services.AddEasyCaching(options =>
            {
                options.UseInMemory(Configuration);

                options.UseCSRedis(Configuration)
                    .WithMessagePack();
            });
            services.AddHostedService<CheckHisotryService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var builder = new ContainerBuilder();
            builder.Populate(services);
            DI.Current = builder.Build();
            return new AutofacServiceProvider(DI.Current);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseEasyCaching();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class CheckHisotryService : BackgroundService
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IEasyCachingProviderFactory _factory;
        private readonly IEasyCachingProvider _redisProvider;
        private readonly IEasyCachingProvider _memoryProvider;
        public CheckHisotryService(ILoggerFactory loggerFactory
        , IEasyCachingProviderFactory factory)
        {
            _logger = loggerFactory.CreateLogger<CheckHisotryService>();
            _factory = factory;
            _memoryProvider = _factory.GetCachingProvider(EasyCachingConstValue.DefaultInMemoryName);
            _redisProvider = _factory.GetCachingProvider(EasyCachingConstValue.DefaultCSRedisName);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                //IEasyCachingProviderFactory _factory = DI.GetService<IEasyCachingProviderFactory>();
                //IEasyCachingProvider _redisProvider = _factory.GetCachingProvider(EasyCachingConstValue.DefaultCSRedisName);
                var res = _redisProvider.Get<string>($"TEST_{Guid.NewGuid().ToString("n")}");
                _logger.LogInformation($"test here {res.HasValue} {System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            catch (Exception ex)
            {
                _logger.LogError("exception here", ex);
                throw ex;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }

    public class DI
    {
        public static IContainer Current { get; set; }
       
        public static bool IsRegistered<T>(string key)
        {
            return Current.IsRegisteredWithKey<T>(key);
        }

        public static bool IsRegistered(Type type)
        {
            return Current.IsRegistered(type);
        }

        public static bool IsRegisteredWithKey(string key, Type type)
        {
            return Current.IsRegisteredWithKey(key, type);
        }

        public static T GetService<T>(string key)
        {

            return Current.ResolveKeyed<T>(key);
        }

        public static object GetService(Type type)
        {
            return Current.Resolve(type);
        }

        public static object GetService(string key, Type type)
        {
            return Current.ResolveKeyed(key, type);
        }
    }
}
