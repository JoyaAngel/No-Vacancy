using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class Pedido
    {
        [Key]
        public int idPedido { get; set; }
        public string? rfc { get; set; }
        public string? regimen { get; set; }
        public string? codigoPostal { get; set; }

        // Dirección de envío
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Colonia { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? CodigoPostalEnvio { get; set; }

        // Fecha y hora del pedido
        public DateTime FechaHoraPedido { get; set; } = DateTime.Now;

        [ForeignKey("Carrito")]
        [Required]
        public int idCarrito { get; set; }
        public CarritoCabecera? Carrito { get; set; }

    }
}
