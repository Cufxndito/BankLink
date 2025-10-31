using BankLink.Data;
using BankLink.Models;
using BankLink.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services.Implementations
{
    public class CuentaService : ICuentaService
    {
        private readonly BankLinkContext _context;

        public CuentaService(BankLinkContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cuenta>> GetAllAsync()
        {
            // Incluimos datos del cliente propietario
            return await _context.Cuentas
                .Include(c => c.Cliente)
                .ToListAsync();
        }

        public async Task<Cuenta?> GetByIdAsync(int id)
        {
            return await _context.Cuentas
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cuenta>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Cuentas
                .Where(c => c.ClienteId == clienteId)
                .Include(c => c.Cliente)
                .ToListAsync();
        }

        public async Task<Cuenta> CreateAsync(Cuenta cuenta)
        {
            // Validar que el cliente exista antes de crear la cuenta
            var cliente = await _context.Clientes.FindAsync(cuenta.ClienteId);
            if (cliente == null)
                throw new Exception("El cliente asociado no existe.");

            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }

        public async Task<Cuenta?> UpdateAsync(int id, Cuenta cuenta)
        {
            var existing = await _context.Cuentas.FindAsync(id);
            if (existing == null)
                return null;

            existing.NumeroCuenta = cuenta.NumeroCuenta;
            existing.TipoCuenta = cuenta.TipoCuenta;
            existing.Activa = cuenta.Activa;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null)
                return false;

            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id, bool activa)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null)
                return false;

            cuenta.Activa = activa;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
