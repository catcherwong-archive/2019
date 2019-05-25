namespace BizLayer.UserInfo
{
    using Dtos;
    using System.Threading.Tasks;

    public interface IUserInfoBiz
    {
        Task<(int code, string msg)> CreateUserAsync(CreateUserDto dto);

        Task<(int code, string msg)> ModifyPasswordAsync(ModifyPasswordDto dto);

        Task<(int code, string msg)> DeleteUserAsync(DeleteUserDto dto);
    }
}
