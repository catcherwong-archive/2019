namespace BizLayer.UserInfo
{
    using CoreLayer.Repositories;
    using Dtos;
    using Notify;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class UserInfoBiz : IUserInfoBiz
    {
        private readonly ILogger _logger;
        private readonly IUserInfoRepository _repo;
        private readonly INotifyBiz _notifyBiz;

        public UserInfoBiz(ILoggerFactory loggerFactory, IUserInfoRepository repo, INotifyBiz notifyBiz)
        {
            _logger = loggerFactory.CreateLogger<UserInfoBiz>();
            _repo = repo;
            _notifyBiz = notifyBiz;
        }

        public async Task<(int code, string msg)> CreateUserAsync(CreateUserDto dto)
        {
            // ignore some params check...


            // here can use AutoMapper to impore
            var userInfo = dto.GetUserInfo();

            var isStrongPassword = userInfo.CheckIsStrongPassword();

            if (!isStrongPassword) return (1001, "password is too weak");

            var isSucc = await _repo.CreateUserAsync(userInfo);

            if (isSucc)
            {
                await _notifyBiz.SendEmail(userInfo.Email);
                _logger.LogInformation("create userinfo succeed..");
                return (0, "ok");
            }
            else
            {
                _logger.LogWarning("create userinfo fail..");
                return (9000, "error");
            }
        }

        public async Task<(int code, string msg)> DeleteUserAsync(DeleteUserDto dto)
        {
            // ignore some params check...

            var userInfo = await _repo.GetUserInfoByUserNameAsync(dto.UserName);

            if(userInfo == null) return (2001, "can not find user");

            var status = userInfo.CheckUserStatus();

            if(status) return (2002, "user is already been deleted");          

            var isSucc = await _repo.DeleteUserAsync(userInfo);

            if (isSucc)
            {
                _logger.LogInformation($"delete {dto.UserName} succeed..");
                return (0, "ok");
            }
            else
            {
                _logger.LogWarning($"delete {dto.UserName} fail..");
                return (9000, "error");
            }
        }

        public async Task<(int code, string msg)> ModifyPasswordAsync(ModifyPasswordDto dto)
        {
            // ignore some params check...

            var userInfo = await _repo.GetUserInfoByUserNameAsync(dto.UserName);

            if (userInfo == null) return (2001, "can not find user");

            userInfo.ModifyPassword(dto.Password);

            var isStrongPassword = userInfo.CheckIsStrongPassword();

            if (!isStrongPassword) return (1001, "password is too weak");

            var isSucc = await _repo.ModifyUserAsync(userInfo);

            if (isSucc)
            {
                _logger.LogInformation($"modify password of {dto.UserName} succeed..");
                return (0, "ok");
            }
            else
            {
                _logger.LogWarning($"modify password of {dto.UserName} fail..");
                return (9000, "error");
            }
        }
    }
}
