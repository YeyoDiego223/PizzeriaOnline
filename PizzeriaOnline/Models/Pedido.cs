using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; }

        [Required]
        public string NombreCliente { get; set; }
        public string DireccionEntrega { get; set; }
        public string Telefono { get; set; }

        public decimal TotalPedido { get; set; }
        public string Estado { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        [Required]
        public string MetodoPago { get; set; }

        // 1. Esta es la ÚNICA declaración de la propiedad.
        public virtual ICollection<DetallePedido> Detalles { get; set; }

        // 2. El constructor le da un valor inicial a la propiedad de arriba.
        public Pedido()
        {
            FechaPedido = DateTime.Now;
            Detalles = new List<DetallePedido>(); // Aquí se inicializa
        }

    }
}
