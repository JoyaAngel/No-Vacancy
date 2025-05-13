using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class CarritoCabecera
    {
        [Key]
        public int idCarrito { get; init; }

        [ForeignKey("Cliente")]
        public int idCliente { get; set; }
        public Cliente Cliente { get; set; }

    }
}