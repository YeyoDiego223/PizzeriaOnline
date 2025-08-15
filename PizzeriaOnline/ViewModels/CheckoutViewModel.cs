using PizzeriaOnline.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class CheckoutViewModel
    {
        // --- Datos para mostrar el resumen del pedido ---i
        public List<CarritoItem> Carrito { get; set; } = new List<CarritoItem>();
        public decimal TotalCarrito { get; set; }
        
        // --- Datos que el cliente llenara en el formulario ---
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string DireccionEntrega { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Indroduce un número de teléfono valido.")]
        public string Telefono { get; set; }

        public List<ProductoExtra> ExtrasDisponibles { get; set; } = new List<ProductoExtra>();
        public List<CarritoExtraViewModel> CarritoExtras { get; set; } = new List<CarritoExtraViewModel>();
        public double Latitud {  get; set; }
        public double Longitud { get; set; }

        public string MetodoPago { get; set; }
    }
}
