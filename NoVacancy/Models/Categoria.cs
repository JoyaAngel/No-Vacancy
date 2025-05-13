using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Categoria
    {
        [Key]
        public int idCategoria { get; set; }
        [Required]
        public string nombre { get; set; }
    }
}
