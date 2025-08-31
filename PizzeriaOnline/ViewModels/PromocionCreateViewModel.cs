using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PromocionCreateViewModel
    {
        [Required(ErrorMessage = "El Título es obligatorio.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La Descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Precio es obligatoria.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La Imagen es obligatoria.")]
        public IFormFile? RutaImagen { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool EstaActiva { get; set; }
    }
}
