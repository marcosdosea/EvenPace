using Core.Service;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EvenPace.Service
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UsuarioIdentity> _signInManager;
        private readonly UserManager<UsuarioIdentity> _userManager;

        public AuthService(
            SignInManager<UsuarioIdentity> signInManager,
            UserManager<UsuarioIdentity> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> LoginAsync(string email, string senha)
        {
            var result = await _signInManager.PasswordSignInAsync(
                email,
                senha,
                isPersistent: false,
                lockoutOnFailure: false
            );

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GetUsuarioLogadoEmailAsync(ClaimsPrincipal user)
        {
            var identityUser = await _userManager.GetUserAsync(user);
            return identityUser?.Email;
        }
    }
}