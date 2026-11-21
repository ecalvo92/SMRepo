using System.ComponentModel.DataAnnotations;

namespace SM_API.Models
{
    public class ConsultarUsuarioRequestModel
    {
        [Required]
        public int IdUsuario { get; set; }
    }
}
