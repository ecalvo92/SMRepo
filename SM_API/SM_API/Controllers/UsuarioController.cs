using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API.Models;

namespace SM_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _configuration) : ControllerBase
    {

        [HttpPut]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@UsuarioId", model.UsuarioId);
            parametros.Add("@Contrasenna", model.ContrasennaNueva);

            var response = context.Execute("sp_ActualizarContrasenna", parametros);

            if (response <= 0)
                return UnprocessableEntity("No se actualizó la información correctamente");

            return Ok("Se actualizó la información correctamente");
        }

        [HttpPost]
        [Route("ConsultarUsuario")]
        public IActionResult ConsultarUsuario(ConsultarUsuarioRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@UsuarioId", model.IdUsuario);

            var response = context.QueryFirstOrDefault<UsuarioResponse>("sp_ConsultarIdUsuario", parametros);

            if (response == null)
                return UnprocessableEntity("No se encontró el usuario");

            return Ok(response);
        }    

    }
}
