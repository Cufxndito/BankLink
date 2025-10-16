using Microsoft.EntityFrameworkCore;
using BankLink.Models;

namespace BankLink.Data;
    public class BankLinkContext : DbContext
    {
        public BankLinkContext(DbContextOptions<BankLinkContext> options)
            : base(options)
        {
            
        }

        // Tablas (DbSet)
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<BancoExterno> BancosExternos { get; set; }
        public DbSet<Transferencia> Transferencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ejemplo: relaciones o restricciones adicionales (opcional)
            modelBuilder.Entity<Cuenta>()
                .HasOne(c => c.Cliente)
                .WithMany(c => c.Cuentas)
                .HasForeignKey(c => c.ClienteId);

            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Cuenta)
                .WithMany(c => c.Movimientos)
                .HasForeignKey(m => m.CuentaId);
        }
    }
