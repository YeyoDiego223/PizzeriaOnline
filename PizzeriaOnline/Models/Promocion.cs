using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.Models
{
    public class Promocion
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Precio { get; set; }
        
        public string? RutaImagen { get; set; }

        public bool EstaActiva { get; set; }
    }
}
