using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EvenPaceWeb.Areas.Identity.Data;

namespace EvenPaceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<UsuarioIdentity> _userManager;

        public HomeController(UserManager<UsuarioIdentity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (usuario is not null && await _userManager.IsInRoleAsync(usuario, "Organizacao"))
                    return RedirectToAction("Index", "Organizacao");

                return RedirectToAction("IndexUsuario", "Evento");
            }

            return View();
        }
    }
}
