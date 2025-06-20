namespace PizzeriaOnline.Models
{
    public class CarritoItem
    {
        public int PizzaId { get; set; }
        public string NombrePizza { get; set; }
        public int TamañoId { get; set; }
        public string NombreTamaño { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
