namespace XXXService
{
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ABasedService : IABasedService
    {
        private readonly ILogger _logger;
        private readonly IFindService _findService;

        public ABasedService(ILoggerFactory loggerFactory, IFindService findService)
        {
            _logger = loggerFactory.CreateLogger<ABasedService>();
            _findService = findService;
        }

        private async Task<(UserInfoService.UserInfoServiceClient Client, string Msg)> GetClientAsync(string name)
        {
            var target = await _findService.FindServiceAsync(name);
            _logger.LogInformation($"Current target = {target}");

            if (string.IsNullOrWhiteSpace(target))
            {
                return (null, "can not find a service");
            }
            else
            {
                var channel = new Channel(target, ChannelCredentials.Insecure);

                var client = new UserInfoService.UserInfoServiceClient(channel);
                return (client, string.Empty);
            }
        }

        public async Task<(DemoUserInfo i, string msg)> GetByIdAsync(int id)
        {
            var (client, msg) = await GetClientAsync("MyBasedServiceA");

            if (!string.IsNullOrWhiteSpace(msg))
            {
                return (null, msg);
            }

            try
            {
                var res = await client.GetByIdAsync(new GetUserByIdRequest { Id = id }, new CallOptions());

                _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(res));

                return (new DemoUserInfo
                {
                    Id = res.Id,
                    Name = res.Name
                }, string.Empty);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "rpc error");
                return (null, ex.StatusCode.ToString());
            }
        }

        public async Task<(List<DemoUserInfo> l, string msg)> GetListAsync(int id, string name)
        {
            var (client, msg) = await GetClientAsync("MyBasedServiceA");

            if (!string.IsNullOrWhiteSpace(msg))
            {
                return (null, msg);
            }

            try
            {
                var res = await client.GetListAsync(new GetUserListRequest
                {
                    Id = id,
                    Name = name,
                }, new CallOptions());

                _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(res));

                if (res.Code.Equals(0))
                {
                    return (res.Data.Select(x => new DemoUserInfo { Id = x.Id, Name = x.Name }).ToList(), string.Empty);
                }
                else
                {
                    return (null, "error");
                }
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "rpc error");
                return (null, ex.StatusCode.ToString());
            }
        }

        public async Task<(bool f, string msg)> SaveAsync(string name, int age)
        {
            var (client, msg) = await GetClientAsync("MyBasedServiceA");

            if (!string.IsNullOrWhiteSpace(msg))
            {
                return (false, msg);
            }

            try
            {
                var res = await client.SaveAsync(new SaveUserRequest
                {
                    Age = age,
                    Name = name,
                }, new CallOptions());

                _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(res));

                return (res.Code.Equals(0), string.Empty);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "rpc error");
                return (false, ex.StatusCode.ToString());
            }
        }
    }
}
