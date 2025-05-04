using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Imagen
    {

        public int idImagen { get; set; }

        [Required]
        public string? nombre { get; set; }

    }
}
