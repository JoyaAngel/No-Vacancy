using System.ComponentModel.DataAnnotations;

namespace NoVacancy.Models
{
    public class Cliente
    {

        public int idCliente { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string correo { get; set; }

        [Required]
        public string constrasenia { get; set; }

    }
}
