using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SM_WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
