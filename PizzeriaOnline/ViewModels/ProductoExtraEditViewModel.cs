using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class ProductoExtraEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 10000.00)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, 10000)]
        public int CantidadEnStock { get; set; }

        // Para mostrar la imagen que ya existe
        public string? RutaImagenExistente { get; set; }
        
        // Para recibir la nueva imagen (es opcional al editar)
        public IFormFile? ImagenArchivo {  get; set; }
    }
}
