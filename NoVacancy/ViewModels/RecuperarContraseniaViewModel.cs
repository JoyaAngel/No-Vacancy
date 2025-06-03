using System.ComponentModel.DataAnnotations;

namespace NoVacancy.ViewModels
{
    public class RecuperarContraseniaViewModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
    }
}
