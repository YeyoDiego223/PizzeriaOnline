namespace PizzeriaOnline.Models
{
    public class PizzaIngrediente
    {
        // Foreign Key para Pizza
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }
        
        // Foreign Key para Ingrediente
        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; }
    }
}
