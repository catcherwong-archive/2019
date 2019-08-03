namespace XXXService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IABasedService
    {
        Task<(DemoUserInfo i, string msg)> GetByIdAsync(int id);

        Task<(bool f, string msg)> SaveAsync(string name, int age);

        Task<(List<DemoUserInfo> l, string msg)> GetListAsync(int id, string name);
    } 
}
