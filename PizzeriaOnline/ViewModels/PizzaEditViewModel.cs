using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PizzaEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        public string? RutaImagenExistente { get; set; }
        public IFormFile ImagenArchivo { get; set; }
    }
}
