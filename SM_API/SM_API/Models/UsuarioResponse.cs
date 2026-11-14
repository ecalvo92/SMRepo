namespace SM_API.Models
{
    public class UsuarioResponse
    {
        public int IdUsuario { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public string Contrasenna { get; set; } = string.Empty;
        public short IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}
