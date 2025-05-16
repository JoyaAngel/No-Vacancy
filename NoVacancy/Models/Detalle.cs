using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoVacancy.Models
{
    public class Detalle
    {
        [Key]
        public int idDetalle { get; set; }
        public double monto { get; set; }

        [ForeignKey("Pedido")]
        public int idPedido { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
