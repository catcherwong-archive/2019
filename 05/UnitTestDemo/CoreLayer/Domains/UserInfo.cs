namespace CoreLayer.Domains
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsDel { get; set; }        

        public bool CheckIsStrongPassword()
        {
            return !(string.IsNullOrWhiteSpace(Password) || Password.Length < 5);
        }

        public void ModifyPassword(string pwd)
        {
            this.Password = pwd;
        }

        public void DeleteUser()
        {
            this.IsDel = true;
        }

        public bool CheckUserStatus()
        {
            return IsDel;
        }
    }
}
