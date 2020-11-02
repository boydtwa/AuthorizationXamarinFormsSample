using System.Threading.Tasks;

namespace AuthXamSam.Services
{
    public interface IMessaging
    {
        Task ShowMessageAsync(string Title, string Message);
    }
}
