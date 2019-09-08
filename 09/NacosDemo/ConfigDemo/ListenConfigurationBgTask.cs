namespace ConfigDemo
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Nacos;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ListenConfigurationBgTask : BackgroundService
    {
        private readonly ILogger _logger;

        private readonly INacosConfigClient _configClient;

        public ListenConfigurationBgTask(ILoggerFactory loggerFactory, INacosConfigClient configClient)
        {
            _logger = loggerFactory.CreateLogger<ListenConfigurationBgTask>();

            _configClient = configClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Add listener
            await _configClient.AddListenerAsync(new AddListenerRequest
            {
                DataId = "demo1",
                //Group = "DEFAULT_GROUP",
                //Tenant = "tenant",
                Callbacks = new List<Action<string>>
                {
                    x =>
                    {
                        _logger.LogInformation($" We found something changed!!! {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}  [{x}]");
                    },
                }
            });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Remove listener
            await _configClient.RemoveListenerAsync(new RemoveListenerRequest
            {
                DataId = "demo1",
                Callbacks = new List<Action>
                {
                    () =>
                    {                        
                         _logger.LogInformation($" Removed listerner  ");
                    },
                }
            });

            await base.StopAsync(cancellationToken);
        }
    }
}
