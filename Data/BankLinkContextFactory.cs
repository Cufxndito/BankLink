//EF Core usa esta clase solo en tiempo de diseño/cuando hacemos migraciones. Para poder crear una instancia del contexto sin depender del Program

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BankLink.Data
{
    public class BankLinkContextFactory : IDesignTimeDbContextFactory<BankLinkContext>
    {
        public BankLinkContext CreateDbContext(string[] args)
        {
            // Cargar configuración desde appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BankLinkContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new BankLinkContext(optionsBuilder.Options);
        }
    }
}
