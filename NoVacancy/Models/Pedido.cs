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

        [ForeignKey("Carrito")]
        [Required]
        public int idCarrito { get; set; }
        public CarritoCabecera Carrito { get; set; }

    }
}
