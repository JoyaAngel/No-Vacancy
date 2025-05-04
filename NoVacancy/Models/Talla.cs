using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Talla
    {

        public int idTalla { get; set; }

        [Required]
        public string? nombre { get; set; }

    }
}
