using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class CarritoCabecera
    {
        [Key]
        public int idCarrito { get; init; }

        [ForeignKey("Usuario")]
        public String Id { get; set; }
        public Usuario? Usuario{ get; set; }

    }
}