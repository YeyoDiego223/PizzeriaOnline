// ViewModels/PizzaCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PizzaCreateViewModel
    {
        [Required(ErrorMessage = "El nombre de la pizza es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debes subir al menos una imagen.")]
        [Display(Name = "Imágenes de la Pizza")]
        public List<IFormFile> ImagenesArchivo { get; set; }
    }
}