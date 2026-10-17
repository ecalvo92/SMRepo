using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API.Models;
using SM_API.Services;
using System.Security.Cryptography;
using System.Text;

namespace SM_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController(IConfiguration _configuration, IPasswordService _password) : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            
            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);

            var response = context.QueryFirstOrDefault<UsuarioResponse>("sp_IniciarSesionUsuario", parametros);

            if(response == null)
                return NotFound("No se validó la información correctamente");

            return Ok(response);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Identificacion", model.Identificacion);
            parametros.Add("@NombreCompleto", model.NombreCompleto);
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);           

            var response = context.Execute("sp_RegistroUsuarios", parametros);

            if(response <= 0)
                return UnprocessableEntity("No se registró la información correctamente");

            return Ok("Se registró la información correctamente");
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Identificacion", model.Identificacion);

            var response = context.QueryFirstOrDefault<UsuarioResponse>("sp_ConsultarIdentificacion", parametros);

            if (response == null)
                return NotFound("No se validó la información correctamente");

            var usuarioId = _password.Cifrar(response.IdUsuario.ToString(), TimeSpan.FromMinutes(30));
            var linkRecuperacion = _configuration.GetValue<string>("ConnectionStrings:UrlRecuperacion") + usuarioId;

            //3. Enviar correo electrónico con el link de recuperación

            return Ok(model);
        }
    }
}
