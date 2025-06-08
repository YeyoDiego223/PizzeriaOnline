using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.Models
{
    public class Tamaño
    {
        public int Id { get; set; }
        [Required]       
        public string Nombre { get; set; }
        public string Dimensiones { get; set; }
        public int NumeroRebanadas { get; set; }
        public int MaximoSabores { get; set; }
        public decimal PrecioBase { get; set; }

        public virtual ICollection<VariantePizza> Variantes { get; set; }
    }
}
