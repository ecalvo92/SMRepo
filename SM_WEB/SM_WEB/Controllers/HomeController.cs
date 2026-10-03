using Microsoft.AspNetCore.Mvc;
using SM_WEB.Models;
using System.Diagnostics;

namespace SM_WEB.Controllers
{
    public class HomeController(HttpClient _httpClient) : Controller
    {
        #region Inicio de Sesión

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UsuarioModel model)
        {
            using var client = _httpClient;
            var url = "https://localhost:7259/api/Home/Login";

            var response = client.PostAsJsonAsync(url, model).Result;

            //Validar response

            return View();
        }

        #endregion

        #region Registro de Usuarios

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UsuarioModel model)
        {
            using var client = _httpClient;
            var url = "https://localhost:7259/api/Home/Login";

            var response = client.PostAsJsonAsync(url, model).Result;

            //Validar response

            return View();
        }

        #endregion

        #region Recuperar la Contraseña

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(UsuarioModel model)
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
