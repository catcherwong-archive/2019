namespace BizLayer.UserInfo.Dtos
{
    using CoreLayer.Domains;

    public class ModifyPasswordDto
    {
        public string UserName { get; set; }    

        public string Password { get; set; }
    }
}
