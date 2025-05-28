using System.ComponentModel.DataAnnotations;

namespace NoVacancy.ViewModels
{
    public class RegistroUsuarioViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es v�lido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contrase�a es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Rol { get; set; }
    }
}
