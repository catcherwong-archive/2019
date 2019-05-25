namespace BizLayer.UserInfo.Dtos
{
    using CoreLayer.Domains;

    public class CreateUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                UserName = UserName,
                Email = Email,
                Password = Password,
                IsDel = false
            };
        }
    }
}
