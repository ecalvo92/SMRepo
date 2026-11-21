using Microsoft.AspNetCore.Mvc;
using SM_WEB.Models;
using System.Net;
using System.Net.Http.Headers;

namespace SM_WEB.Controllers
{
    public class UsuarioController(HttpClient _httpClient, IConfiguration _configuration) : Controller
    {
        #region Cambiar Datos Personales

        [HttpGet]
        public IActionResult CambiarDatos()
        {
            var (datosPersonales, mensaje) = ConsultarDatosPersonales();

            if(datosPersonales != null)
                return View(new PerfilUsuarioViewModel { DatosPersonales = datosPersonales });

            ViewBag.Mensaje = mensaje;
            return View(new PerfilUsuarioViewModel());
        }

        [HttpPost]
        public IActionResult CambiarDatos([Bind(Prefix = "Contrasenna")]ChangePasswordRequestModel model)
        {
            if(!ModelState.IsValid)
                return View(new PerfilUsuarioViewModel 
                { 
                    Contrasenna = model, 
                    DatosPersonales = ConsultarDatosPersonales().Datos ?? new() 
                });

            model.UsuarioId = HttpContext.Session.GetInt32("ConsecutivoUsuario").ToString()!;

            //Hacer un hash de la contraseña nueva y de la confirmación
            model.ContrasennaNueva = BCrypt.Net.BCrypt.HashPassword(model.ContrasennaNueva);
            model.ConfirmarContrasenna = BCrypt.Net.BCrypt.HashPassword(model.ConfirmarContrasenna);

            using var client = _httpClient;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Usuario/ChangePassword";

            var response = client.PutAsJsonAsync(url, model).Result;

            ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
            return View(new PerfilUsuarioViewModel 
            { DatosPersonales = ConsultarDatosPersonales().Datos ?? new() });
        }

        [HttpPost]
        public IActionResult ActualizarDatosPersonales([Bind(Prefix = "DatosPersonales")] DatosPersonalesRequestModel model)
        {
            if (!ModelState.IsValid)
                return View("CambiarDatos", new PerfilUsuarioViewModel { DatosPersonales = model });

            model.IdUsuario = HttpContext.Session.GetInt32("ConsecutivoUsuario").GetValueOrDefault();

            using var client = _httpClient;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Usuario/ActualizarDatosPersonales";

            var response = client.PutAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContext.Session.SetString("Nombre", model.NombreCompleto);
            }

            ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
            return View("CambiarDatos", new PerfilUsuarioViewModel { DatosPersonales = model });
        }

        #endregion

        private (DatosPersonalesRequestModel? Datos, string? Mensaje) ConsultarDatosPersonales()
        {
            var model = new ConsultarUsuarioRequestModel
            {
                IdUsuario = HttpContext.Session.GetInt32("ConsecutivoUsuario").GetValueOrDefault()
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _configuration.GetValue<string>("Variables:ApiBaseUrl") + "Usuario/ConsultarUsuario";

            var response = _httpClient.PostAsJsonAsync(url, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content.ReadFromJsonAsync<UsuarioResponse>().Result;
                var perfil = new PerfilUsuarioViewModel()!;
                perfil.DatosPersonales.CorreoElectronico = datos!.CorreoElectronico;
                perfil.DatosPersonales.Identificacion = datos!.Identificacion;
                perfil.DatosPersonales.NombreCompleto = datos!.NombreCompleto;
                return (perfil.DatosPersonales, null);
            }
            else
            {
                var mensaje = response.Content.ReadAsStringAsync().Result;
                return (null, mensaje);
            }
        }
    }
}
