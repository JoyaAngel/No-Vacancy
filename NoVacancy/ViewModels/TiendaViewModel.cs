using System.Collections.Generic;
using NoVacancy.Models;

namespace NoVacancy.ViewModels
{
    public class ProductoTiendaViewModel
    {
        public Producto? Producto { get; set; }
        public List<Imagen>? Imagenes { get; set; }
    }

    public class TiendaViewModel
    {
        public List<ProductoTiendaViewModel>? Productos { get; set; }
        public List<Categoria>? Categorias { get; set; } = new List<Categoria>();
        public List<Color>? Colores { get; set; } = new List<Color>();
        public List<Talla>? Tallas { get; set; } = new List<Talla>();
        public int TotalPaginas { get; set; } = 1;
        public int PaginaActual { get; set; } = 1;
    }
}
