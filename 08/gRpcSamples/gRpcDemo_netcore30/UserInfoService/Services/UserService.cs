namespace UserInfoService
{
    using System.Linq;
    using System.Threading.Tasks;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;

    public class UserService : UserInfoRpcService.UserInfoRpcServiceBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override async Task<GetUserByIdRelpy> GetById(GetUserByIdRequest request, ServerCallContext context)
        {
            //context.CancellationToken.Register(() =>
            //{
            //    _logger.LogInformation("what happen here");
            //    //context.CancellationToken.ThrowIfCancellationRequested();
            //});            
            //await Task.Delay(2000);

            await Task.Delay(2000, context.CancellationToken);
            
            var result = new GetUserByIdRelpy();

            var user = FakeUserInfoDb.GetById(request.Id);

            result.Id = user.Id;
            result.Name = user.Name;
            result.Age = user.Age;
            result.CreateTime = user.CreateTime;

            return result;
        }

        public override Task<GetUserListReply> GetList(GetUserListRequest request, ServerCallContext context)
        {
            var result = new GetUserListReply();

            var userList = FakeUserInfoDb.GetList(request.Id, request.Name);

            result.Code = 0;
            result.Msg = "成功";
            result.Data.AddRange(userList.Select(x => new GetUserListReply.Types.MsgItem
            {
                Id = x.Id,
                Age = x.Age,
                CreateTime = x.CreateTime,
                Name = x.Name
            }));

            return Task.FromResult(result);
        }

        public override Task<SaveUserReply> Save(SaveUserRequest request, ServerCallContext context)
        {
            var result = new SaveUserReply();

            var flag = FakeUserInfoDb.Save(request.Name, request.Age);

            result.Code = 0;
            result.Msg = "成功";

            return Task.FromResult(result);
        }
    }
}
