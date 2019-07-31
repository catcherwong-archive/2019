namespace XXXService
{
    using System.Threading.Tasks;

    public interface IFindService
    {
        Task<string> FindServiceAsync(string serviceName);        
    }
}
