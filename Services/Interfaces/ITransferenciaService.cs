using BankLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankLink.Services.Interfaces
{
    public interface ITransferenciaService
    {
        // 🔹 Listar todas las transferencias
        Task<IEnumerable<Transferencia>> GetAllAsync();

        // 🔹 Obtener una transferencia por ID
        Task<Transferencia?> GetByIdAsync(int id);

        // 🔹 Transferencia entre cuentas del mismo banco
        Task<Transferencia> TransferirInternaAsync(int cuentaOrigenId, int cuentaDestinoId, decimal monto, string descripcion);

        // 🔹 Transferencia hacia otro banco
        Task<string> TransferirExternaAsync(int cuentaOrigenId, string numeroCuentaDestinoExterna, decimal monto, string descripcion, string urlBancoDestino);

        // 🔹 Eliminar (opcional)
        Task<bool> DeleteAsync(int id);
    }
}
