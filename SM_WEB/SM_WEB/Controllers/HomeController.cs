using Microsoft.AspNetCore.Mvc;
using SM_WEB.Models;
using System.Diagnostics;

namespace SM_WEB.Controllers
{
    public class HomeController(HttpClient _httpClient) : Controller
    {
        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(UsuarioModel model)
        {
            using var client = _httpClient;
            var url = "https://localhost:7259/api/Home/Login";

            var response = client.PostAsJsonAsync(url, model).Result;

            //Validar response

            return View();
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
