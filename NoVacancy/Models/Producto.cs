using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class Producto
    {

        [Key]
        public int idProducto { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public int cantidad { get; set; }
        public int? limite { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public double precio { get; set; }
        
        [ForeignKey("Talla")]
        [Required]
        public int idTalla { get; set; }
        public Talla Talla { get; set; }

        [ForeignKey("Categoria")]
        [Required]
        public int idCategoria { get; set; }
        public Categoria Categoria { get; set; }

        [ForeignKey("Color")]
        [Required]
        public int idColor { get; set; }
        public Color Color { get; set; }

    }
}
