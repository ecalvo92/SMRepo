using Microsoft.AspNetCore.Mvc;
using SM_WEB.Models;
using System.Diagnostics;

namespace SM_WEB.Controllers
{
    public class HomeController(HttpClient _httpClient, IConfiguration _configuration) : Controller
    {
        #region Inicio de Sesión

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginRequestModel model)
        {
            using var client = _httpClient;
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Home/Login";

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
        public IActionResult Register(RegisterRequestModel model)
        {
            using var client = _httpClient;
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Home/Register";

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
        public IActionResult ForgotPassword(ForgotRequestModel model)
        {
            using var client = _httpClient;
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Home/ForgotPassword";

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
