namespace BizLayer.Notify
{
    using System.Threading.Tasks;

    public interface INotifyBiz
    {
        Task<bool> SendEmail(string email);
    }
}
