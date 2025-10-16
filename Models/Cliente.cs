using System.ComponentModel.DataAnnotations;

namespace BankLink.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [Required, MaxLength(50)]
        public string Apellido { get; set; }

        [Required, MaxLength(15)]
        public string Dni { get; set; }

        [MaxLength(100)]
        public string Direccion { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        // Relación: un cliente puede tener muchas cuentas
        public ICollection<Cuenta> Cuentas { get; set; }
    }
}
