using BankLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLink.Services.Interfaces
{
    public interface ICuentaService
    {
        Task<IEnumerable<Cuenta>> GetAllAsync();
        Task<Cuenta?> GetByIdAsync(int id);
        Task<IEnumerable<Cuenta>> GetByClienteIdAsync(int clienteId);
        Task<Cuenta> CreateAsync(Cuenta cuenta);
        Task<Cuenta?> UpdateAsync(int id, Cuenta cuenta);
        Task<bool> DeleteAsync(int id);
        Task<bool> CambiarEstadoAsync(int id, bool activa);
    }
}

