using System.ComponentModel.DataAnnotations;

namespace NoVacancy.ViewModels
{
    public class RestablecerContraseniaViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NuevaContrasenia { get; set; }

        [Required(ErrorMessage = "Confirma la nueva contraseña")]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasenia", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasenia { get; set; }
    }
}
