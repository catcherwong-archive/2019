namespace CoreLayer.Repositories
{
    using Domains;
    using System.Threading.Tasks;

    public interface IUserInfoRepository
    {
        Task<bool> CreateUserAsync(UserInfo userInfo);

        Task<bool> ModifyUserAsync(UserInfo userInfo);

        Task<bool> DeleteUserAsync(UserInfo userInfo);

        Task<UserInfo> GetUserInfoByUserNameAsync(string userName);
    }
}
