using System;
using System.Threading.Tasks;
using AuthXamSam.Models;

namespace AuthXamSam.Services
{
    public interface IMicrosoftAuthService
    {
        void Initialize();
        Task<User> OnSignInAsync();
        Task OnSignOutAsync();
    }
}
