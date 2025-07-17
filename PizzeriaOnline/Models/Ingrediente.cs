using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaOnline.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del ingrediente es obligatorio.")]
        public string Nombre { get; set; }

        // Usamos 'double' para poder manejar decimales (ej: 1.5 Kilos)
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
        public decimal CantidadEnStock { get; set; }

        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        public string UnidadDeMedida { get; set; }

    }
}
