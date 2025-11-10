using Microsoft.AspNetCore.Mvc;

namespace BankLink.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // Usuario y contraseña fijos (solo para demo)
            const string usuarioValido = "admin";
            const string passValida = "1234";

            if (dto.Usuario == usuarioValido && dto.Contraseña == passValida)
            {
                // En un sistema real devolverías un JWT o cookie
                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso",
                    token = "TOKEN-DEMO-12345"
                });
            }

            return Unauthorized(new { mensaje = "Credenciales inválidas" });
        }
    }
}
