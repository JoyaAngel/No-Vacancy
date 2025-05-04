using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Color
    {

        public int idColor { get; set; }

        [Required]
        public string? nombre { get; set; }


    }
}
