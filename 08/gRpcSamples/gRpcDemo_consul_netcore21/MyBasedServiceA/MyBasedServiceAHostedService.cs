namespace MyBasedServiceA
{
    using Consul;
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class MyBasedServiceAHostedService : BackgroundService
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IConsulClient _consulClient;

        private Server _server;
        private AgentServiceRegistration registration;

        public MyBasedServiceAHostedService(ILoggerFactory loggerFactory, IConfiguration configuration, IConsulClient consulClient, IHostingEnvironment environment)
        {
            this._logger = loggerFactory.CreateLogger<MyBasedServiceAHostedService>();
            this._configuration = configuration;
            this._consulClient = consulClient;

            var port = _configuration.GetValue<int>("AppSettings:Port");

            _logger.LogInformation($"{environment.EnvironmentName} Current Port is : {port}");

            // global logger for grpc
            GrpcEnvironment.SetLogger(new GrpcAdapterLogger(loggerFactory));

            var address = GetLocalIP();

            registration = new AgentServiceRegistration()
            {
                ID = $"MyBasedServiceA-{port}",
                Name = "MyBasedServiceA",
                Address = address,
                Port = port,
                Check = new AgentServiceCheck
                {
                    TCP = $"{address}:{port}",
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5)
                } 
            };

            _server = new Server
            {
                Ports = { new ServerPort("0.0.0.0", port, ServerCredentials.Insecure) }                
            };

            // not production record some things
            if (!environment.IsProduction())
            {
                _server.Services.Add(UserInfoService.BindService(new UserInfoServiceImpl()).Intercept(new AccessLogInterceptor(loggerFactory)));
            }
            else
            {
                _server.Services.Add(UserInfoService.BindService(new UserInfoServiceImpl()));
            }
        }
      
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Registering with Consul");
            await _consulClient.Agent.ServiceDeregister(registration.ID);
            await _consulClient.Agent.ServiceRegister(registration);

            _server.Start();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unregistering from Consul");

            //await _consulClient.Agent.ServiceDeregister(registration.ID);
            //await _server.KillAsync();
            await base.StopAsync(cancellationToken);
        }

        private string GetLocalIP()
        {
            try
            {
                string hostName = Dns.GetHostName(); 
                IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < ipEntry.AddressList.Length; i++)
                {
                    if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ipEntry.AddressList[i].ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Local Ip error");
                return "127.0.0.1";
            }
        }
    }
}
