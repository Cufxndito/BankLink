using BankLink.Data;
using BankLink.Models;
using BankLink.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankLink.Services.Implementations
{
    public class BancoExternoService : IBancoExternoService
    {
        private readonly BankLinkContext _context;

        public BancoExternoService(BankLinkContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BancoExterno>> GetAllAsync()
        {
            return await _context.BancosExternos.ToListAsync();
        }

        public async Task<BancoExterno?> GetByIdAsync(int id)
        {
            return await _context.BancosExternos.FindAsync(id);
        }

        public async Task<BancoExterno> CreateAsync(BancoExterno banco)
        {
            _context.BancosExternos.Add(banco);
            await _context.SaveChangesAsync();
            return banco;
        }

        public async Task<BancoExterno?> UpdateAsync(int id, BancoExterno banco)
        {
            var existing = await _context.BancosExternos.FindAsync(id);
            if (existing == null)
                return null;

            existing.NombreBanco = banco.NombreBanco;
            existing.CodigoIdentificacion = banco.CodigoIdentificacion;
            existing.UrlBaseApi = banco.UrlBaseApi;
            existing.EndpointTransferencias = banco.EndpointTransferencias;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var banco = await _context.BancosExternos.FindAsync(id);
            if (banco == null)
                return false;

            _context.BancosExternos.Remove(banco);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
