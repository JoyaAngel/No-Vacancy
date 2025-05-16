using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class Imagen
    {

        [Key]
        public int idImagen { get; set; }
        [Required]
        public string? nombre { get; set; }
        
        [ForeignKey("Producto")]
        [Required]
        public int idProducto { get; set; }
        public Producto? Producto { get; set; }

    }
}
