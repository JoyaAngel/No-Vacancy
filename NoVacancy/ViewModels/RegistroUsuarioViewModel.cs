using System.ComponentModel.DataAnnotations;

namespace NoVacancy.ViewModels
{
    public class RegistroUsuarioViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(AutoGenerateField = false)]
        public string Rol { get; set; } = "Cliente";

        // Dirección
        [StringLength(100, ErrorMessage = "La calle no debe exceder 100 caracteres.")]
        public string? Calle { get; set; }
        [StringLength(10, ErrorMessage = "El número no debe exceder 10 caracteres.")]
        public string? Numero { get; set; }
        [StringLength(100, ErrorMessage = "La colonia no debe exceder 100 caracteres.")]
        public string? Colonia { get; set; }
        [StringLength(100, ErrorMessage = "La ciudad no debe exceder 100 caracteres.")]
        public string? Ciudad { get; set; }
        [StringLength(100, ErrorMessage = "El estado no debe exceder 100 caracteres.")]
        public string? Estado { get; set; }
        [RegularExpression(@"^\d{5}$", ErrorMessage = "El código postal debe tener 5 dígitos.")]
        public string? CodigoPostal { get; set; }
    }
}
