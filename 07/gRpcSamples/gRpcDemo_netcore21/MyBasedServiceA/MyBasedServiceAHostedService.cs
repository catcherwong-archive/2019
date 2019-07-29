namespace MyBasedServiceA
{
    using Grpc.Core;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MyBasedServiceAHostedService : BackgroundService
    {
        private readonly ILogger _logger;

        private Server _server;

        public MyBasedServiceAHostedService(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<MyBasedServiceAHostedService>();

            GrpcEnvironment.SetLogger(new GrpcLogger(_logger));

            _server = new Server
            {
                Services = { UserInfoService.BindService(new UserInfoServiceImpl()) },
                Ports = { new ServerPort("0.0.0.0", 9999, ServerCredentials.Insecure) }                
            };
        }
      
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _server.Start();
            return Task.CompletedTask;
        }
    }

    public class GrpcLogger : Grpc.Core.Logging.ILogger
    {

        private readonly ILogger _logger;

        public GrpcLogger(ILogger logger)
        {
            this._logger = logger;
        }

        public Grpc.Core.Logging.ILogger ForType<T>()
        {
            return this;
        }

        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }

        public void Debug(string format, params object[] formatArgs)
        {
            _logger.LogDebug(format);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Error(string format, params object[] formatArgs)
        {
            _logger.LogError(format);
        }

        public void Error(Exception exception, string message)
        {
            _logger.LogError(exception, message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Info(string format, params object[] formatArgs)
        {
            _logger.LogInformation(format);
        }

        public void Warning(string message)
        {
            _logger.LogWarning(message);
        }

        public void Warning(string format, params object[] formatArgs)
        {
            _logger.LogWarning(format);
        }

        public void Warning(Exception exception, string message)
        {
            _logger.LogWarning(exception, message);
        }
    }
}
