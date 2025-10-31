using Microsoft.AspNetCore.Mvc;
using BankLink.Services.Interfaces;
using BankLink.Models;

namespace BankLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BancosExternosController : ControllerBase
    {
        private readonly IBancoExternoService _bancoExternoService;

        public BancosExternosController(IBancoExternoService bancoExternoService)
        {
            _bancoExternoService = bancoExternoService;
        }

        // GET: api/bancosexternos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bancos = await _bancoExternoService.GetAllAsync();
            return Ok(bancos);
        }

        // GET: api/bancosexternos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var banco = await _bancoExternoService.GetByIdAsync(id);
            if (banco == null)
                return NotFound(new { mensaje = "Banco externo no encontrado" });

            return Ok(banco);
        }

        // POST: api/bancosexternos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BancoExterno nuevoBanco)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creado = await _bancoExternoService.CreateAsync(nuevoBanco);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        // PUT: api/bancosexternos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BancoExterno bancoActualizado)
        {
            if (id != bancoActualizado.Id)
                return BadRequest(new { mensaje = "El ID del banco no coincide" });

            var actualizado = await _bancoExternoService.UpdateAsync(id, bancoActualizado);
            if (actualizado == null)
                return NotFound(new { mensaje = "Banco externo no encontrado" });

            return Ok(actualizado);
        }

        // DELETE: api/bancosexternos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _bancoExternoService.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Banco externo no encontrado" });

            return NoContent();
        }
    }
}
