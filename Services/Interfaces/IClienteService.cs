using BankLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLink.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<Cliente?> UpdateAsync(int id, Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}
