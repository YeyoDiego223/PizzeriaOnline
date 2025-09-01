namespace PizzeriaOnline.Models
{
    public class PizzaImagen
    {
        public int Id { get; set; }
        public string RutaImagen { get; set; }
        public bool EsImagenPrincipal { get; set; }

        public int PizzaId { get; set; }
        public virtual Pizza Pizza { get; set; }
    }
}
