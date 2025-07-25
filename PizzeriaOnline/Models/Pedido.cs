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
        public double? Latitud {  get; set; }
        public double? Longitud { get; set; }

        //Propiedad de navegación: Un pedido tiene MUCHOS detalles.
        public virtual ICollection<DetallePedido> Detalles { get; set; }
        public Pedido() 
        {
            FechaPedido = DateTime.Now;
            Detalles = new List<DetallePedido>();
        }
    }
}
