using Microsoft.AspNetCore.Mvc;

namespace Future.Web.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}