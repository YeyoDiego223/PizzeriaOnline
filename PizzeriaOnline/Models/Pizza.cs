namespace PizzeriaOnline.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Pizza
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen{ get; set; }

        public virtual ICollection<PizzaIngrediente> PizzaIngredientes { get; set; }
        public virtual ICollection<DetalleSabor> DetalleSabores { get; set; }
    }
}
