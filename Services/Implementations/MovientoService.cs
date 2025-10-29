using BankLink.Data;
using BankLink.Models;
using BankLink.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services.Implementations
{
    public class MovimientoService : IMovimientoService
    {
        private readonly BankLinkContext _context;

        public MovimientoService(BankLinkContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movimiento>> GetAllAsync()
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.Cliente)
                .ToListAsync();
        }

        public async Task<Movimiento?> GetByIdAsync(int id)
        {
            return await _context.Movimientos
                .Include(m => m.Cuenta)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Movimiento>> GetByCuentaIdAsync(int cuentaId)
        {
            return await _context.Movimientos
                .Where(m => m.CuentaId == cuentaId)
                .OrderByDescending(m => m.FechaHora)
                .ToListAsync();
        }

        public async Task<Movimiento> DepositarAsync(int cuentaId, decimal monto, string descripcion)
        {
            var cuenta = await _context.Cuentas.FindAsync(cuentaId);
            if (cuenta == null)
                throw new Exception("La cuenta no existe.");

            if (!cuenta.Activa)
                throw new Exception("La cuenta está inactiva.");

            cuenta.SaldoActual += monto;

            var movimiento = new Movimiento
            {
                CuentaId = cuentaId,
                TipoMovimiento = "Depósito",
                Monto = monto,
                Descripcion = descripcion,
                FechaHora = DateTime.Now
            };

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            return movimiento;
        }

        public async Task<Movimiento?> RetirarAsync(int cuentaId, decimal monto, string descripcion)
        {
            var cuenta = await _context.Cuentas.FindAsync(cuentaId);
            if (cuenta == null)
                throw new Exception("La cuenta no existe.");

            if (!cuenta.Activa)
                throw new Exception("La cuenta está inactiva.");

            if (cuenta.SaldoActual < monto)
                throw new Exception("Saldo insuficiente para realizar el retiro.");

            cuenta.SaldoActual -= monto;

            var movimiento = new Movimiento
            {
                CuentaId = cuentaId,
                TipoMovimiento = "Retiro",
                Monto = monto,
                Descripcion = descripcion,
                FechaHora = DateTime.Now
            };

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            return movimiento;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
                return false;

            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
