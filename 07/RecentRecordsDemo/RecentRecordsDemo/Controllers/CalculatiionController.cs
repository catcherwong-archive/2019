namespace RecentRecordsDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using EasyCaching.Core;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Triggered by scheduling system
    /// </summary>
    [Route("api/cal")]
    [ApiController]
    public class CalculatiionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRedisCachingProvider _provider;

        public CalculatiionController(ILoggerFactory loggerFactory, IRedisCachingProvider provider)
        {
            _logger = loggerFactory.CreateLogger<CalculatiionController>();
            _provider = provider;
        }

        // GET api/cal/
        [HttpGet]
        public string Get()
        {
            var id = Guid.NewGuid().ToString("N");

            _ = Task.Run(async () => await CalAsync(id));

            return "ok";
        }

        private async Task CalAsync(string id)
        {
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
    }
}
