using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace PizzeriaOnline.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string NombreTamaño { get; set; }

        public virtual ICollection<DetalleSabor> DetalleSabores { get; set; }

        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }

        public DetallePedido()
        {
            DetalleSabores = new List<DetalleSabor>();
        }
    }
}
