namespace PizzeriaOnline.Models
{
    public class VariantePizza
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public virtual Pizza Pizza { get; set; }
        public int TamañoId { get; set; }
        public virtual Tamaño Tamaño { get; set; }
    }
}
