using System.ComponentModel.DataAnnotations;

namespace SM_WEB.Models
{
    public class ForgotRequestModel
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; } = string.Empty;
    }
}
