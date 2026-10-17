using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SM_WEB.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View("Error");
        }
    }
}
