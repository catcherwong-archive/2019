namespace XXXService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IABasedService
    {
        Task<DemoUserInfo> GetByIdAsync(int id);

        Task<bool> SaveAsync(string name, int age);

        Task<List<DemoUserInfo>> GetListAsync(int id, string name);
    } 
}
