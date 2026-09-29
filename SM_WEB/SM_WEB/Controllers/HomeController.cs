using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SM_WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
