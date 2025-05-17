using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Usuario
    {

        public int idUsuario { get; set; }

        [Required]
        public string? nombre { get; set; }

        [Required]
        public string? correo { get; set; }

        [Required]
        public string? constrasenia { get; set; }

        [Required]
        public string? rol { get; set; }

    }
}
