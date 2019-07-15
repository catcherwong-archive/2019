namespace RecentRecordsDemo.BgTasks
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EasyCaching.Core;
    using System.Threading;

    /// <summary>
    /// Calculate kpi via recent records
    /// </summary>
    public class CalculateDataSourceKpiBgTask : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IRedisCachingProvider _provider;
        private Timer _timer;
        private bool _polling;

        public CalculateDataSourceKpiBgTask(ILoggerFactory loggerFactory, IRedisCachingProvider provider)
        {
            this._logger = loggerFactory.CreateLogger<CalculateDataSourceKpiBgTask>();
            this._provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"start backgroud taks ...");

            _timer = new Timer(async x =>
            {
                if (_polling)
                {
                    _logger.LogInformation($"latest manipulation is still working ...");
                    return;
                }
                _polling = true;
                await PollAsync();
                _polling = false;
            }, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(10));

            return Task.CompletedTask;
        }

        private async Task PollAsync()
        {
            var id = Guid.NewGuid().ToString("N");

            _logger.LogInformation($"{id} begin at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

          
            var datasourceIds = GetAllDataSourceIds();

            foreach (var item in datasourceIds)
            {
                try
                {
                    var topN = await _provider.LRangeAsync<DataSourceInfo>($"info:{item}", 0, 99);

                    var cost = topN.Average(x => x.Cost);
                    var rate = topN.Count(x => x.IsSucceed) / 100;

                    var score = GetScore(cost, rate);

                    await _provider.HSetAsync($"dskpi", item, score.ToString());

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{id} {item} calculate fail ...");
                }
            }

            _logger.LogInformation($"{id} end at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        private int GetScore(double cost, int rate)
        {
            return new Random().Next(1, 100);
        }

        private List<string> GetAllDataSourceIds()
        {
            return new List<string> { "100", "900" };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"stop backgroud taks ...");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
