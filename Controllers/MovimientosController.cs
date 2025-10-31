using Microsoft.AspNetCore.Mvc;
using BankLink.Services.Interfaces;
using BankLink.Models;

namespace BankLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientoService _movimientoService;

        public MovimientosController(IMovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        // GET: api/movimientos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movimientos = await _movimientoService.GetAllAsync();
            return Ok(movimientos);
        }

        // GET: api/movimientos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movimiento = await _movimientoService.GetByIdAsync(id);
            if (movimiento == null)
                return NotFound(new { mensaje = "Movimiento no encontrado" });

            return Ok(movimiento);
        }

        // GET: api/movimientos/por-cuenta/3
        [HttpGet("por-cuenta/{cuentaId}")]
        public async Task<IActionResult> GetByCuentaId(int cuentaId)
        {
            var movimientos = await _movimientoService.GetByCuentaIdAsync(cuentaId);
            return Ok(movimientos);
        }

        // POST: api/movimientos/deposito
        [HttpPost("deposito")]
        public async Task<IActionResult> Depositar([FromBody] MovimientoRequest request)
        {
            try
            {
                var movimiento = await _movimientoService.DepositarAsync(
                    request.CuentaId,
                    request.Monto,
                    request.Descripcion
                );

                return Ok(new
                {
                    mensaje = "Depósito realizado con éxito",
                    movimiento
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // POST: api/movimientos/retiro
        [HttpPost("retiro")]
        public async Task<IActionResult> Retirar([FromBody] MovimientoRequest request)
        {
            try
            {
                var movimiento = await _movimientoService.RetirarAsync(
                    request.CuentaId,
                    request.Monto,
                    request.Descripcion
                );

                if (movimiento == null)
                    return BadRequest(new { mensaje = "No se pudo realizar el retiro" });

                return Ok(new
                {
                    mensaje = "Retiro realizado con éxito",
                    movimiento
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // DELETE: api/movimientos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _movimientoService.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Movimiento no encontrado" });

            return NoContent();
        }
    }

    // DTO para depósitos y retiros
    public class MovimientoRequest
    {
        public int CuentaId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
