using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API.Models;

namespace SM_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRequestModel model)
        {
            using var context = new SqlConnection("Server=PC?; Database=SM_DB?; Trusted_Connection=True;TrustServerCertificate=True;");
            
            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);

            var response = context.Query("SP", parametros);

            return Ok(model);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterRequestModel model)
        {
            using var context = new SqlConnection("Server=PC?; Database=SM_DB?; Trusted_Connection=True;TrustServerCertificate=True;");

            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);
            parametros.Add("@Identificacion", model.Identificacion);
            parametros.Add("@NombreCompleto", model.NombreCompleto);

            var response = context.Execute("SP", parametros);

            return Ok(model);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotRequestModel model)
        {
            using var context = new SqlConnection("Server=PC?; Database=SM_DB?; Trusted_Connection=True;TrustServerCertificate=True;");

            var parametros = new DynamicParameters();
            parametros.Add("@Identificacion", model.Identificacion);

            var response = context.Query("SP", parametros);

            return Ok(model);
        }

    }
}
