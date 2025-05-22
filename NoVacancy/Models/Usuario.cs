using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoVacancy.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Rol { get; set; }

        // Puedes agregar otras propiedades personalizadas
        // como FechaRegistro, EsPremium, etc.
    }
}
