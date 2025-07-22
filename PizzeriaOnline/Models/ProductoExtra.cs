using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.Models
{
    public class ProductoExtra
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, 10000, ErrorMessage = "La cantidad no puede ser negativa.")]
        public int CantidadEnStock { get; set; }

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        public string RutaImagen { get; set; }
    }
}
