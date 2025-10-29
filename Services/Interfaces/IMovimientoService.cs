using BankLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLink.Services.Interfaces
{
    public interface IMovimientoService
    {
        Task<IEnumerable<Movimiento>> GetAllAsync();
        Task<Movimiento?> GetByIdAsync(int id);
        Task<IEnumerable<Movimiento>> GetByCuentaIdAsync(int cuentaId);

        Task<Movimiento> DepositarAsync(int cuentaId, decimal monto, string descripcion);
        Task<Movimiento?> RetirarAsync(int cuentaId, decimal monto, string descripcion);

        Task<bool> DeleteAsync(int id);
    }
}
