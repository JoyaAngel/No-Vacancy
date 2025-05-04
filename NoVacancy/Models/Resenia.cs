using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Resenia
    {

        public int idResenia { get; set; }

        [Required]
        public string? resenia { get; set; }

        [Required]
        public int? calificacion { get; set; }

    }
}
