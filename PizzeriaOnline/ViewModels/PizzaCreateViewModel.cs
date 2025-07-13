// ViewModels/PizzaCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PizzaCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La ruta de la imagen es obligatoria.")]
        public string RutaImagen { get; set; }
    }
}