namespace SM_WEB.Models
{
    public class PerfilUsuarioViewModel
    {
        public DatosPersonalesRequestModel DatosPersonales { get; set; } = new();

        public ChangePasswordRequestModel Contrasenna { get; set; } = new();
    }
}
