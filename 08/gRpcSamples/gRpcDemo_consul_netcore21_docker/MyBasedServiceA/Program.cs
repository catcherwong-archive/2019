namespace MyBasedServiceA
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .UseEnvironment(Microsoft.AspNetCore.Hosting.EnvironmentName.Development)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {                    
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);

                    configApp.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })                  
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddConsul(hostContext.Configuration);                    
                    services.AddHostedService<MyBasedServiceAHostedService>();
                })
                .Build();

            host.Run();
        }
    }
}
