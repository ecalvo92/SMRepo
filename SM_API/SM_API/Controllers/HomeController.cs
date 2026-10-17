using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API.Models;
using System.Security.Cryptography;
using System.Text;

namespace SM_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController(IConfiguration _configuration) : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            
            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);

            var response = context.QueryFirstOrDefault<UsuarioResponse>("sp_IniciarSesionUsuario", parametros);

            if(response == null)
                return NotFound("No se validó la información correctamente");

            return Ok(response);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Identificacion", model.Identificacion);
            parametros.Add("@NombreCompleto", model.NombreCompleto);
            parametros.Add("@CorreoElectronico", model.CorreoElectronico);
            parametros.Add("@Contrasenna", model.Contrasenna);           

            var response = context.Execute("sp_RegistroUsuarios", parametros);

            if(response <= 0)
                return UnprocessableEntity("No se registró la información correctamente");

            return Ok("Se registró la información correctamente");
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotRequestModel model)
        {
            using var context = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Identificacion", model.Identificacion);

            var response = context.QueryFirstOrDefault<UsuarioResponse>("sp_ConsultarIdentificacion", parametros);

            if (response == null)
                return NotFound("No se validó la información correctamente");

            var usuarioId = Cifrar(response.IdUsuario.ToString(), TimeSpan.FromMinutes(30));
            var linkRecuperacion = _configuration.GetValue<string>("ConnectionStrings:UrlRecuperacion") + usuarioId;

            //3. Enviar correo electrónico con el link de recuperación

            return Ok(model);
        }

        private readonly byte[] Key = Convert.FromBase64String("sy+v61jJHH5tZTjth2g+PwKpfUiNXWA0ImlUWxikTKUedfPEC6zrkDSzsneTbfkTXJnaEchH7BcVBF4XkqIMDw==");

        public string Cifrar(string userId, TimeSpan vigencia)
        {
            var payload = Encoding.UTF8.GetBytes($"{userId}|{DateTime.UtcNow.Add(vigencia).Ticks}");
            using (var aes = Aes.Create())
            using (var hmac = new HMACSHA256(Key.AsSpan(32).ToArray()))
            {
                aes.Key = Key.AsSpan(0, 32).ToArray();
                aes.GenerateIV();
                var cifrado = aes.CreateEncryptor().TransformFinalBlock(payload, 0, payload.Length);

                var datos = new byte[16 + cifrado.Length + 32];
                Buffer.BlockCopy(aes.IV, 0, datos, 0, 16);
                Buffer.BlockCopy(cifrado, 0, datos, 16, cifrado.Length);
                Buffer.BlockCopy(hmac.ComputeHash(datos, 0, 16 + cifrado.Length), 0, datos, 16 + cifrado.Length, 32);

                return Convert.ToBase64String(datos).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            }
        }

        public string Descifrar(string token)
        {
            try
            {
                var b64 = token.Replace('-', '+').Replace('_', '/');
                var datos = Convert.FromBase64String(b64.PadRight(b64.Length + (4 - b64.Length % 4) % 4, '='));
                if (datos.Length < 16 + 16 + 32) return string.Empty;

                int largoMac = datos.Length - 32;
                using (var hmac = new HMACSHA256(Key.AsSpan(32).ToArray()))
                {
                    var macEsperado = hmac.ComputeHash(datos, 0, largoMac);
                    int diff = 0;
                    for (int i = 0; i < 32; i++) diff |= macEsperado[i] ^ datos[largoMac + i];
                    if (diff != 0) return string.Empty;
                }

                using (var aes = Aes.Create())
                {
                    aes.Key = Key.AsSpan(0, 32).ToArray();
                    aes.IV = datos.AsSpan(0, 16).ToArray();
                    var plano = aes.CreateDecryptor().TransformFinalBlock(datos, 16, largoMac - 16);
                    var partes = Encoding.UTF8.GetString(plano).Split('|');

                    if (DateTime.UtcNow.Ticks > long.Parse(partes[1])) return string.Empty;
                    return partes[0];
                }
            }
            catch { return string.Empty; }
        }

    }
}
