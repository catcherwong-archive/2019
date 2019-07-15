namespace RecentRecordsDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using EasyCaching.Core;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;
    using System;

    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRedisCachingProvider _provider;

        public AreaController(ILoggerFactory loggerFactory, IRedisCachingProvider provider)
        {
            _logger = loggerFactory.CreateLogger<AreaController>();
            _provider = provider;
        }

        // GET api/area/11
        [HttpGet("provinceId")]
        public async Task<string> GetAsync(string provinceId)
        {
            // get datasorce
            var datasource = await GetQueryDataSourceIdAsync(provinceId);

            if (string.IsNullOrWhiteSpace(datasource)) return "not support";

            var beginTime = DateTime.Now;
           
            var (val, isSucceed) = await QueryDataSourceAsync(datasource);

            var endTime = DateTime.Now;

            var dsInfo = new DataSourceInfo
            {
                Cost = (long)endTime.Subtract(endTime).TotalMilliseconds,
                IsSucceed = isSucceed
            };

            _ = Task.Run(async () =>
            {
                try
                {
                    await _provider.LPushAsync($"info:{datasource}", new List<DataSourceInfo> { dsInfo });
                    await _provider.LTrimAsync($"info:{datasource}", 0, 99);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"record #{datasource}# error");
                }
            });

            return val;
        }

        private async Task<string> GetQueryDataSourceIdAsync(string provinceId)
        {
            var datasourceIds = GetDataSourceIdProvinceId(provinceId);

            if (datasourceIds.Count <= 0) return string.Empty;
           
            var cacheKey = "dskpi";

            var kpis = await _provider.HMGetAsync(cacheKey, datasourceIds);

            var datasource = datasourceIds.First();

            if (kpis != null && kpis.Any())
            {
                datasource = kpis.OrderByDescending(x => x.Value).First().Key;
            }

            return datasource;
        }

        private async Task<(string val, bool isSucceed)> QueryDataSourceAsync(string datasource)
        {
            await Task.Delay(100);

            var rd = new Random().NextDouble();

            return (datasource, rd > 0.5d);
        }

        private List<string> GetDataSourceIdProvinceId(string provinceId)
        {
            return new List<string> { "100", "900" };
        }
    }
}
