namespace PizzeriaOnline.ViewModels
{
    public class CarritoExtraViewModel
    {
        public int ProductoExtraId { get; set; }
        public string Nombre { get; set; }
        public int Cantidad {  get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
