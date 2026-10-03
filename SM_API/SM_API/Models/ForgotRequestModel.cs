using System.ComponentModel.DataAnnotations;

namespace SM_API.Models
{
    public class ForgotRequestModel
    {
        [Required]
        public string Identificacion { get; set; } = string.Empty;
    }
}
