using System.ComponentModel.DataAnnotations;

namespace SM_WEB.Models
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
