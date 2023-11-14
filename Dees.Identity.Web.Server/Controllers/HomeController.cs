using Microsoft.AspNetCore.Mvc;

namespace Dees.Identity.Web.Server
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
