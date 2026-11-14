using System.ComponentModel.DataAnnotations;

namespace SM_API.Models
{
    public class ChangePasswordRequestModel
    {
        [Required]
        public string ContrasennaNueva { get; set; } = string.Empty;

        [Required]
        public string UsuarioId { get; set; } = string.Empty;
    }
}
