using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class ProductoExtraCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int CantidadEnStock { get; set; }
    }
}
