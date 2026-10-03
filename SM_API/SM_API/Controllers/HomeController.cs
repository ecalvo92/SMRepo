using Microsoft.AspNetCore.Mvc;
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
            //Autenticación contra la base de datos

            return Ok(model);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterRequestModel model)
        {
            //Registro contra la base de datos

            return Ok(model);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotRequestModel model)
        {
            //Revisar si el usuario existe GET

            //Actualizar contraseña por una clave temporal PUT

            //Enviarle un correo al usuario con la clave temporal POST

            return Ok(model);
        }

    }
}
