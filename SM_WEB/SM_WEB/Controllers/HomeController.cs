using Microsoft.AspNetCore.Mvc;
using SM_WEB.Models;
using System.Diagnostics;
using System.Net;

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
            if (!ModelState.IsValid)
                return View(model);

            using var client = _httpClient;
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Home/Login";

            var response = client.PostAsJsonAsync(url, model).Result;

            if(response.StatusCode == HttpStatusCode.OK)
                return RedirectToAction("Index", "Home");

            ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
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
            if (!ModelState.IsValid)
                return View(model);

            using var client = _httpClient;
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Home/Register";

            var response = client.PostAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
                return RedirectToAction("Login", "Home");

            ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
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
            if(!ModelState.IsValid)
                return View(model);

            using var client = _httpClient;
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Home/ForgotPassword";

            var response = client.PostAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
                return RedirectToAction("Login", "Home");

            ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
            return View();
        }

        #endregion

        #region Cambiar la Contraseña

        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

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
