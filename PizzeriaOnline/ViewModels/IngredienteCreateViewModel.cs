// ViewModels/IngredienteCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class IngredienteCreateViewModel
    {
        [Required(ErrorMessage = "El nombre del ingrediente es obligatorio.")]
        public string Nombre { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
        public double CantidadEnStock { get; set; }

        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        public string UnidadDeMedida { get; set; }
    }
}