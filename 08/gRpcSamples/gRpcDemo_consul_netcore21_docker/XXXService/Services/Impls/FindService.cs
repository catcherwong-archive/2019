namespace XXXService
{
    using Consul;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;

    public class FindService : IFindService 
    {
        private readonly ILogger _logger;
        private readonly IConsulClient _consulClient;
        private readonly ConcurrentDictionary<string, (List<string> List, DateTimeOffset Expiration)> _dict;

        public FindService(ILoggerFactory loggerFactory, IConsulClient consulClient)
        {
            _logger = loggerFactory.CreateLogger<FindService>();
            _consulClient = consulClient;
            _dict = new ConcurrentDictionary<string, (List<string> List, DateTimeOffset Expiration)>();
        }

        public async Task<string> FindServiceAsync(string serviceName)
        {
            var key = $"SD:{serviceName}";

            if (_dict.TryGetValue(key, out var item) && item.Expiration > DateTimeOffset.UtcNow)
            {
                _logger.LogInformation($"Read from cache");
                return item.List[new Random().Next(0, item.List.Count)];              
            }
            else
            {
                var queryResult = await _consulClient.Health.Service(serviceName, string.Empty, true);

                var result = new List<string>();
                foreach (var serviceEntry in queryResult.Response)
                {
                    result.Add(serviceEntry.Service.Address + ":" + serviceEntry.Service.Port);
                }

                _logger.LogInformation($"Read from consul : {string.Join(",", result)}");

                if (result != null && result.Any())
                {
                    // for demonstration, we make expiration a little big
                    var val = (result, DateTimeOffset.UtcNow.AddSeconds(600));

                    _dict.AddOrUpdate(key, val, (x, y) => val);

                    var count = result.Count;
                    return result[new Random().Next(0, count)];
                }

                return "";
            }
        }
    }
}
