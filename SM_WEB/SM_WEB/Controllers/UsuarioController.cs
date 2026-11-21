using Microsoft.AspNetCore.Mvc;

namespace SM_WEB.Controllers
{
    public class UsuarioController : Controller
    {
        #region Cambiar Datos Personales

        [HttpGet]
        public IActionResult CambiarDatos()
        {
            return View();
        }

        #endregion
    }
}
