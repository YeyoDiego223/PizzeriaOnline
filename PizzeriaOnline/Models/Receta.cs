namespace PizzeriaOnline.Models
{
    public class Receta
    {
        // Conexión con la Pizza (el sabor)
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }

        // Conexión con el tamaño
        public int TamañoId { get; set; }
        public Tamaño Tamaño { get; set; }

        // Conexión con el ingrediente
        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; }

        // La cantidad de este ingrediente para esta pizza en este tamaño
        public decimal Cantidad { get; set; }
    }
}
