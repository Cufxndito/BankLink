using Microsoft.AspNetCore.Mvc;
using BankLink.Services.Interfaces;
using BankLink.Models;
using BankLink.Models.DTOs;

namespace BankLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : ControllerBase
    {
        private readonly ITransferenciaService _transferenciaService;

        public TransferenciasController(ITransferenciaService transferenciaService)
        {
            _transferenciaService = transferenciaService;
        }

        // GET: api/transferencias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transferencias = await _transferenciaService.GetAllAsync();
            return Ok(transferencias);
        }

        // GET: api/transferencias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transferencia = await _transferenciaService.GetByIdAsync(id);
            if (transferencia == null)
                return NotFound(new { mensaje = "Transferencia no encontrada" });

            return Ok(transferencia);
        }

        // POST: api/transferencias/interna
        [HttpPost("interna")]
        public async Task<IActionResult> TransferirInterna([FromBody] TransferenciaRequestInterna request)
        {
            try
            {
                var transferencia = await _transferenciaService.TransferirInternaAsync(
                    request.CuentaOrigenId,
                    request.CuentaDestinoId,
                    request.Monto,
                    request.Descripcion
                );

                return Ok(new
                {
                    mensaje = "Transferencia interna realizada con éxito",
                    transferencia
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST: api/transferencias/externa
        [HttpPost("externa")]
        public async Task<IActionResult> TransferirExterna([FromBody] TransferenciaRequestExterna request)
        {
            try
            {
                var resultado = await _transferenciaService.TransferirExternaAsync(
                    request.CuentaOrigenId,
                    request.NumeroCuentaDestinoExterna,
                    request.Monto,
                    request.Descripcion,
                    request.UrlBancoDestino
                );

                return Ok(new { mensaje = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST: api/transferencias/recibir
        [HttpPost("recibir")]
        public async Task<IActionResult> RecibirTransferenciaExterna([FromBody] TransferenciaExternaRequest request)
        {
            try
            {
                var mensaje = await _transferenciaService.RecibirExternaAsync(
                    request.NumeroCuentaDestino,
                    request.Monto,
                    request.Descripcion,
                    request.BancoOrigen
                );

                return Ok(new { mensaje });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // GET: api/transferencias/validar
        [HttpGet("validar")]
        public IActionResult ValidarConexion()
        {
            return Ok(new
            {
                banco = "BankLink",
                codigo = "BKL",
                estado = "OK",
                mensaje = "BankLink operativo y listo para recibir transferencias ✅"
            });
        }

        // DELETE: api/transferencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _transferenciaService.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Transferencia no encontrada" });

            return NoContent();
        }
    }

    // DTOs internos
    public class TransferenciaRequestInterna
    {
        public int CuentaOrigenId { get; set; }
        public int CuentaDestinoId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class TransferenciaRequestExterna
    {
        public int CuentaOrigenId { get; set; }
        public string NumeroCuentaDestinoExterna { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string UrlBancoDestino { get; set; } = string.Empty;
    }
}
