namespace MyBasedServiceA
{
    using Grpc.Core;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserInfoServiceImpl : UserInfoService.UserInfoServiceBase
    {
        public override Task<GetUserByIdRelpy> GetById(GetUserByIdRequest request, ServerCallContext context)
        {
            var result = new GetUserByIdRelpy();

            var user = FakeUserInfoDb.GetById(request.Id);

            result.Id = user.Id;
            result.Name = user.Name;
            result.Age = user.Age;
            result.CreateTime = user.CreateTime;

            return Task.FromResult(result);
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
