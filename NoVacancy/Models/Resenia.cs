using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class Resenia
    {

        [Key]
        public int idResenia { get; set; }
        [Required]
        public string? resenia { get; set; }
        [Required]
        public int calificacion { get; set; }

        [ForeignKey("Producto")]
        [Required]
        public int idProducto { get; set; }
        public Producto? Producto { get; set; }

        [ForeignKey("Pedido")]
        [Required]
        public int idPedido { get; set; }
        public Pedido? Pedido { get; set; }

    }
}
