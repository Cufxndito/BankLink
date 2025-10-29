using BankLink.Data;
using BankLink.Models;
using BankLink.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly BankLinkContext _context;

        public ClienteService(BankLinkContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> UpdateAsync(int id, Cliente cliente)
        {
            var existing = await _context.Clientes.FindAsync(id);
            if (existing == null) return null;

            existing.Nombre = cliente.Nombre;
            existing.Apellido = cliente.Apellido;
            existing.Dni = cliente.Dni;
            existing.Direccion = cliente.Direccion;
            existing.Telefono = cliente.Telefono;
            existing.Email = cliente.Email;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
