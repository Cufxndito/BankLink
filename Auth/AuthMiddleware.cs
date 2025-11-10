using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BankLink.Auth
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        private const string API_KEY = "mi-clave-bkl-12345";
        private const string INTERNAL_TOKEN = "Bearer TOKEN-DEMO-12345";

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Permitir acceso libre a Swagger, favicon y login
            if (path.StartsWith("/swagger") ||
                path.Contains("swagger") ||
                path.Contains("index.html") ||
                path.StartsWith("/favicon.ico") ||
                path.StartsWith("/api/auth"))
            {
                await _next(context);
                return;
            }

            // Permitir llamadas externas (recibir o validar)
            if (path.StartsWith("/api/transferencias/recibir") ||
                path.StartsWith("/api/transferencias/validar"))
            {
                if (!context.Request.Headers.TryGetValue("X-API-KEY", out var providedKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Falta la API Key (X-API-KEY)");
                    return;
                }

                if (providedKey != API_KEY)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("API Key inválida");
                    return;
                }

                await _next(context);
                return;
            }

            // Endpoints internos (cuentas, clientes, etc.)
            if (!context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Falta el header Authorization");
                return;
            }

            if (token != INTERNAL_TOKEN)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token interno inválido");
                return;
            }

            await _next(context);
        }
    }
}
