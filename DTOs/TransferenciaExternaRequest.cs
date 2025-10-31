namespace BankLink.Models.DTOs
{
    public class TransferenciaExternaRequest
    {
        public string BancoOrigen { get; set; } = string.Empty;
        public string NumeroCuentaDestino { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
