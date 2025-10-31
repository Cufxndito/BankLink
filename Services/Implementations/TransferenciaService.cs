using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BankLink.Data;
using BankLink.Models;
using BankLink.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BankLink.Services.Implementations
{
    public class TransferenciaService : ITransferenciaService
    {
        private readonly BankLinkContext _context;
        private readonly HttpClient _httpClient;

        public TransferenciaService(BankLinkContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // Listar todas las transferencias
        public async Task<IEnumerable<Transferencia>> GetAllAsync()
        {
            return await _context.Transferencias
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino)
                .Include(t => t.BancoExterno)
                .ToListAsync();
        }

        // Obtener transferencia por ID
        public async Task<Transferencia?> GetByIdAsync(int id)
        {
            return await _context.Transferencias
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino)
                .Include(t => t.BancoExterno)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Transferencia interna
        public async Task<Transferencia> TransferirInternaAsync(int cuentaOrigenId, int cuentaDestinoId, decimal monto, string descripcion)
        {
            // usamos una transacción para asegurar consistencia
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cuentaOrigen = await _context.Cuentas.FindAsync(cuentaOrigenId);
                var cuentaDestino = await _context.Cuentas.FindAsync(cuentaDestinoId);

                if (cuentaOrigen == null || cuentaDestino == null)
                    throw new Exception("Alguna de las cuentas no existe.");

                if (!cuentaOrigen.Activa || !cuentaDestino.Activa)
                    throw new Exception("Alguna de las cuentas está inactiva.");

                if (cuentaOrigen.SaldoActual < monto)
                    throw new Exception("Saldo insuficiente en la cuenta origen.");

                // actualizar saldos
                cuentaOrigen.SaldoActual -= monto;
                cuentaDestino.SaldoActual += monto;

                // registrar transferencia
                var transferencia = new Transferencia
                {
                    CuentaOrigenId = cuentaOrigenId,
                    CuentaDestinoId = cuentaDestinoId,
                    Monto = monto,
                    Fecha = DateTime.Now,
                    TipoTransferencia = "Interna",
                    Descripcion = descripcion
                };

                _context.Transferencias.Add(transferencia);

                // registrar movimientos
                _context.Movimientos.Add(new Movimiento
                {
                    CuentaId = cuentaOrigenId,
                    TipoMovimiento = "Transferencia Enviada",
                    Monto = monto,
                    FechaHora = DateTime.Now,
                    Descripcion = descripcion
                });

                _context.Movimientos.Add(new Movimiento
                {
                    CuentaId = cuentaDestinoId,
                    TipoMovimiento = "Transferencia Recibida",
                    Monto = monto,
                    FechaHora = DateTime.Now,
                    Descripcion = descripcion
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return transferencia;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Transferencia externa
        public async Task<string> TransferirExternaAsync(int cuentaOrigenId, string numeroCuentaDestinoExterna, decimal monto, string descripcion, string urlBancoDestino)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cuentaOrigen = await _context.Cuentas.FindAsync(cuentaOrigenId);
                if (cuentaOrigen == null)
                    throw new Exception("Cuenta origen no encontrada.");

                if (!cuentaOrigen.Activa)
                    throw new Exception("La cuenta origen está inactiva.");

                if (cuentaOrigen.SaldoActual < monto)
                    throw new Exception("Saldo insuficiente.");

                // descontar dinero
                cuentaOrigen.SaldoActual -= monto;

                // crear el body JSON para el otro banco
                var transferenciaData = new
                {
                    BancoOrigen = "BankLink",
                    NumeroCuentaOrigen = cuentaOrigen.NumeroCuenta,
                    NumeroCuentaDestino = numeroCuentaDestinoExterna,
                    Monto = monto,
                    Descripcion = descripcion
                };

                var json = JsonSerializer.Serialize(transferenciaData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _httpClient.PostAsync($"{urlBancoDestino}/api/transferencias/recibir", content);

                if (!response.IsSuccessStatusCode)
                {
                    // revertir saldo si falla
                    cuentaOrigen.SaldoActual += monto;
                    await _context.SaveChangesAsync();
                    await transaction.RollbackAsync();

                    throw new Exception($"Error al transferir al banco externo: {response.StatusCode}");
                }

                // registrar transferencia local
                var transferencia = new Transferencia
                {
                    CuentaOrigenId = cuentaOrigenId,
                    Monto = monto,
                    Fecha = DateTime.Now,
                    TipoTransferencia = "Externa Enviada",
                    Descripcion = descripcion
                };

                _context.Transferencias.Add(transferencia);

                _context.Movimientos.Add(new Movimiento
                {
                    CuentaId = cuentaOrigenId,
                    TipoMovimiento = "Transferencia Externa Enviada",
                    Monto = monto,
                    FechaHora = DateTime.Now,
                    Descripcion = descripcion
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Transferencia externa enviada con éxito.";
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transferencia = await _context.Transferencias.FindAsync(id);
            if (transferencia == null)
                return false;

            _context.Transferencias.Remove(transferencia);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
