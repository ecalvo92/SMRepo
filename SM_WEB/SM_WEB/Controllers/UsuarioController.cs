using Microsoft.AspNetCore.Mvc;
using SM_WEB.Models;
using System.Net;

namespace SM_WEB.Controllers
{
    public class UsuarioController(HttpClient _httpClient, IConfiguration _configuration) : Controller
    {
        #region Cambiar Datos Personales

        [HttpGet]
        public IActionResult CambiarDatos()
        {
            return View(new PerfilUsuarioViewModel());
        }

        [HttpPost]
        public IActionResult CambiarDatos([Bind(Prefix = "Contrasenna")]ChangePasswordRequestModel model)
        {
            if(!ModelState.IsValid)
                return View(new PerfilUsuarioViewModel { Contrasenna = model });

            model.UsuarioId = HttpContext.Session.GetInt32("ConsecutivoUsuario").ToString()!;

            //Hacer un hash de la contraseña nueva y de la confirmación
            model.ContrasennaNueva = BCrypt.Net.BCrypt.HashPassword(model.ContrasennaNueva);
            model.ConfirmarContrasenna = BCrypt.Net.BCrypt.HashPassword(model.ConfirmarContrasenna);

            using var client = _httpClient;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Usuario/ChangePassword";

            var response = client.PutAsJsonAsync(url, model).Result;

            ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
            return View(new PerfilUsuarioViewModel());
        }

        #endregion
    }
}
