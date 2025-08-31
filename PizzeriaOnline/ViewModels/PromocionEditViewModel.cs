using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PromocionEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Título es obligatorio.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La Descripción es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 10000.00)]
        public decimal Precio { get; set; }        

        // Para mostrar la imagen que ya existe
        public string? RutaImagenExistente { get; set; }

        // Para recibir la nueva imagen (es opcional al editar)
        public IFormFile? ImagenArchivo { get; set; }

        public bool EstaActiva { get; set; }
    }
}
