using Microsoft.AspNetCore.Mvc;
using BankLink.Services.Interfaces;
using BankLink.Models;

namespace BankLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaService _cuentaService;

        public CuentasController(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        // GET: api/Cuentas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cuentas = await _cuentaService.GetAllAsync();
            return Ok(cuentas);
        }

        // GET: api/Cuentas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cuenta = await _cuentaService.GetByIdAsync(id);
            if (cuenta == null)
                return NotFound(new { mensaje = "Cuenta no encontrada" });

            return Ok(cuenta);
        }

        // GET: api/Cuentas/por-cliente/3
        [HttpGet("por-cliente/{clienteId}")]
        public async Task<IActionResult> GetByClienteId(int clienteId)
        {
            var cuentas = await _cuentaService.GetByClienteIdAsync(clienteId);
            return Ok(cuentas);
        }

        // POST: api/Cuentas
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cuenta nuevaCuenta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cuentaCreada = await _cuentaService.CreateAsync(nuevaCuenta);
                return CreatedAtAction(nameof(GetById), new { id = cuentaCreada.Id }, cuentaCreada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // PUT: api/Cuentas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cuenta cuentaActualizada)
        {
            var cuenta = await _cuentaService.UpdateAsync(id, cuentaActualizada);
            if (cuenta == null)
                return NotFound(new { mensaje = "Cuenta no encontrada" });

            return Ok(cuenta);
        }

        // DELETE: api/Cuentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _cuentaService.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Cuenta no encontrada" });

            return NoContent();
        }

        // PATCH: api/Cuentas/5/cambiar-estado
        [HttpPatch("{id}/cambiar-estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] bool activa)
        {
            var resultado = await _cuentaService.CambiarEstadoAsync(id, activa);
            if (!resultado)
                return NotFound(new { mensaje = "Cuenta no encontrada" });

            return Ok(new { mensaje = $"Cuenta {(activa ? "activada" : "desactivada")} correctamente" });
        }
    }
}
