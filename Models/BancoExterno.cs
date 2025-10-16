using System.ComponentModel.DataAnnotations;

namespace BankLink.Models
{
    public class BancoExterno
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string NombreBanco { get; set; }

        [Required, MaxLength(20)]
        public string CodigoIdentificacion { get; set; }

        [Required, MaxLength(200)]
        public string UrlBaseApi { get; set; } // URL del banco externo

        [MaxLength(200)]
        public string EndpointTransferencias { get; set; } // Endpoint para enviar transferencias
    }
}
