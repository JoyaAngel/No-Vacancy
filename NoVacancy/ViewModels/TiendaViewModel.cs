using System.Collections.Generic;
using NoVacancy.Models;

namespace NoVacancy.ViewModels
{
    public class ProductoTiendaViewModel
    {
        public Producto Producto { get; set; }
        public List<Imagen> Imagenes { get; set; }
    }

    public class TiendaViewModel
    {
        public List<ProductoTiendaViewModel> Productos { get; set; }
    }
}
