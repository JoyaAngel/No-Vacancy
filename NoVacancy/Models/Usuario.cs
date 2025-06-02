using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoVacancy.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;

        // Dirección
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Colonia { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? CodigoPostal { get; set; }

    }
}
