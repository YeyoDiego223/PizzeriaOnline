// ViewModels/IngredienteCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class IngredienteCreateViewModel
    {
        [Required(ErrorMessage = "El nombre del ingrediente es obligatorio.")]
        public string Nombre { get; set; }

        [Required]
        [Range(0.01, 1000000.00, ErrorMessage = "La cantidad debe ser un valor positivo.")]
        public decimal CantidadEnStock { get; set; }

        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        public string UnidadDeMedida { get; set; }
    }
}