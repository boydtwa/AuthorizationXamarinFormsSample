using AuthXamSam.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuthXamSam.ViewModels
{
    public interface ILoginViewModel
    {
        bool IsLoading { get; set; }
        ICommand LoginCommand { get; }
        User User { get; set; }

        Task SignInAsync();
        Task SignOutAsync();
    }
}