namespace PizzeriaOnline.Models
{
    public class DetalleSabor
    {

        // Foreign Key para el detalle del pedido (la pizza personalizada)
        public int DetallePedidoId { get; set; }
        public DetallePedido DetallePedido { get; set; }

        // Foreign Key para la pizza (el sabor)
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }
    }
}
