namespace XXXService
{
    using Consul;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var url = configuration.GetValue<string>("AppSettings:ConsulUrl");
                consulConfig.Address = new Uri(url);
            }));

            return services;
        }
    }
}
