using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankLink.Models
{
    public class Movimiento
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cuenta")]
        public int CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }

        [Required, MaxLength(30)]
        public string TipoMovimiento { get; set; } // "Depósito", "Retiro", "Transferencia Enviada", "Transferencia Recibida"

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime FechaHora { get; set; } = DateTime.Now;

        [MaxLength(200)]
        public string Descripcion { get; set; }
    }
}
