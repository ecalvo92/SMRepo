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
        public IActionResult Login(UsuarioModel model)
        {
            //Autenticación contra la base de datos

            return Ok(model);
        }
    }
}
