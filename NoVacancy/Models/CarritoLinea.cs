using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class CarritoLinea
    {

        public int idCarrito { get; set; }
        public CarritoCabecera? Carrito { get; set; }

        public int idProducto { get; set; }
        public Producto? Producto { get; set; }

        [Required]
        public int cantidad { get; set; }

    }
}
