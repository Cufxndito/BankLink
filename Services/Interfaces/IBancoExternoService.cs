using BankLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLink.Services.Interfaces
{
    public interface IBancoExternoService
    {
        Task<IEnumerable<BancoExterno>> GetAllAsync();
        Task<BancoExterno?> GetByIdAsync(int id);
        Task<BancoExterno> CreateAsync(BancoExterno banco);
        Task<BancoExterno?> UpdateAsync(int id, BancoExterno banco);
        Task<bool> DeleteAsync(int id);
    }
}
