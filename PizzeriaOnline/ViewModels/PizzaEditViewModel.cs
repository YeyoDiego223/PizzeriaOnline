using Microsoft.AspNetCore.Http;
using PizzeriaOnline.Models;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PizzaEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la pizza es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        public List<PizzaImagen> ImagenesExistentes { get; set; } = new List<PizzaImagen>();
        public List<int> ImagenesAEliminar { get; set; } = new List<int>();

        [Display(Name = "Añadir Nuevas Imagenes")]
        public List<IFormFile>? ImagenesNuevas { get; set; }

        [Display(Name = "Imagen Principal")]
        public int ImagenPrincipalId { get; set; }
    }
}
