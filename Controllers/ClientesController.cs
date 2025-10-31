using Microsoft.AspNetCore.Mvc;
using BankLink.Services.Interfaces;
using BankLink.Models;

namespace BankLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(cliente);
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente nuevoCliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creado = await _clienteService.CreateAsync(nuevoCliente);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente clienteActualizado)
        {
            if (id != clienteActualizado.Id)
                return BadRequest(new { mensaje = "El ID del cliente no coincide" });

            var actualizado = await _clienteService.UpdateAsync(id, clienteActualizado);
            if (actualizado == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(actualizado);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _clienteService.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return NoContent();
        }
    }
}
