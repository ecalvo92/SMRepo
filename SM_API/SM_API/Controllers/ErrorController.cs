using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API.Services;

namespace SM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController(IConfiguration _configuration, IPasswordService _password) : ControllerBase
    {
        [Route("CapturarError")]
        public IActionResult CapturarError()
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();

            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@IdUsuario", _password.ObtenerConsecutivoToken());
            parametros.Add("@Error", ex!.Error);
            parametros.Add("@Origen", ex!.Path);

            context.Execute("sp_RegistroErrores", parametros);

            return StatusCode(StatusCodes.Status500InternalServerError, "Se presentó un inconveniente técnico.");
        }
    }
}
