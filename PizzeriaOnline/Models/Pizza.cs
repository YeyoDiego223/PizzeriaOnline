namespace PizzeriaOnline.Models
{
    using System.Collections.Generic;
    public class Pizza
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio {  get; set; }
        public string RutaImagen{ get; set; }

        // Propiedad de navegación para la relación muchos a muchos
        public virtual ICollection<Ingredientes> Ingredientes { get; set; }
    }
}
