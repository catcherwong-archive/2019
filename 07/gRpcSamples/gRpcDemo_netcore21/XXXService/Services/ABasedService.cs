namespace XXXService
{
    using Grpc.Core;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ABasedService : IABasedService
    {
        private Channel _channel;
        private UserInfoService.UserInfoServiceClient _client;

        public ABasedService()
        {
            _channel = new Channel("localhost:9999", ChannelCredentials.Insecure);
            _client = new UserInfoService.UserInfoServiceClient(_channel);
        }

        public async Task<DemoUserInfo> GetByIdAsync(int id)
        {
            var res = await _client.GetByIdAsync(new GetUserByIdRequest { Id = id }, new CallOptions());

            return new DemoUserInfo
            {
                Id = res.Id,
                Name = res.Name
            };
        }

        public async Task<List<DemoUserInfo>> GetListAsync(int id, string name)
        {
            var res = await _client.GetListAsync(new GetUserListRequest
            {
                Id = id,
                Name = name,
            }, new CallOptions());

            if (res.Code.Equals(0))
            {
                return res.Data.Select(x => new DemoUserInfo { Id = x.Id, Name = x.Name }).ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SaveAsync(string name, int age)
        {
            var res = await _client.SaveAsync(new SaveUserRequest
            {
                Age = age,
                Name = name,
            }, new CallOptions());

            return res.Code.Equals(0);
        }
    }
}
