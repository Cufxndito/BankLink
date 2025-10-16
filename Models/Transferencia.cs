using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankLink.Models
{
    public class Transferencia
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CuentaOrigen")]
        public int CuentaOrigenId { get; set; }
        public Cuenta CuentaOrigen { get; set; }

        [ForeignKey("CuentaDestino")]
        public int? CuentaDestinoId { get; set; } // Puede ser null si va a banco externo
        public Cuenta CuentaDestino { get; set; }

        [ForeignKey("BancoExterno")]
        public int? BancoExternoId { get; set; } // null si es transferencia interna
        public BancoExterno BancoExterno { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required, MaxLength(20)]
        public string TipoTransferencia { get; set; } // "Enviada" o "Recibida"

        [MaxLength(200)]
        public string Descripcion { get; set; }
    }
}
