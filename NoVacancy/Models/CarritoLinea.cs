using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class CarritoLinea
    {

        [Key]
        [ForeignKey("CarritoCabecera")]
        public int idCarrito { get; init; }
        public CarritoCabecera Carrito { get; set; }

        [Key]
        [ForeignKey("Producto")]
        public int idProducto { get; set; }
        public Producto Producto { get; set; }

        [Required]
        public int cantidad { get; set; }

    }
}
