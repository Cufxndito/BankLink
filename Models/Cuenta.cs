using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankLink.Models
{
    public class Cuenta
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string NumeroCuenta { get; set; }

        [Required, MaxLength(20)]
        public string TipoCuenta { get; set; } // "Ahorro" o "Corriente"

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoActual { get; set; }

        [Required]
        public bool Activa { get; set; } = true;

        // Relación: una cuenta pertenece a un cliente
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // Relación: una cuenta tiene muchos movimientos
        public ICollection<Movimiento> Movimientos { get; set; }
    }
}
