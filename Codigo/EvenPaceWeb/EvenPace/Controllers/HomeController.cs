using Microsoft.AspNetCore.Mvc;

namespace EvenPaceWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
