using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NoVacancy.ViewModels
{
    public class ProductoCreateViewModel
    {
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        public List<VarianteViewModel> Variantes { get; set; } = new List<VarianteViewModel>();
    }

    public class VarianteViewModel : IValidatableObject
    {
        public int? IdColor { get; set; }
        public string? NuevoColor { get; set; }
        public string? NuevoCodigo { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public List<TallaViewModel> Tallas { get; set; } = new List<TallaViewModel>();
        public List<IFormFile> Imagenes { get; set; } = new List<IFormFile>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // If IdColor is null, 0, or 'nuevo', require NuevoColor and NuevoCodigo
            bool isNuevo = !IdColor.HasValue || IdColor == 0;
            // In the form, 'nuevo' is sent as null or 0, so this is sufficient
            if (isNuevo)
            {
                if (string.IsNullOrWhiteSpace(NuevoColor))
                {
                    yield return new ValidationResult("El campo 'Nuevo color' es obligatorio si selecciona agregar nuevo color.", new[] { nameof(NuevoColor) });
                }
                if (string.IsNullOrWhiteSpace(NuevoCodigo))
                {
                    yield return new ValidationResult("El campo 'Código de color' es obligatorio si selecciona agregar nuevo color.", new[] { nameof(NuevoCodigo) });
                }
            }
        }
    }

    public class TallaViewModel
    {
        [Required]
        public int IdTalla { get; set; }
        [Required]
        public int Cantidad { get; set; }
        public int? Limite { get; set; }
    }
}
