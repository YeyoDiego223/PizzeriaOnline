using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaOnline.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Guardaremos una descripción del producto para la posteridad
        public string DescripcionProducto { get; set; }

        // --- Relación con el Pedido ---
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }
    }
}
