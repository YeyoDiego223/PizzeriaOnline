using PizzeriaOnline.Models;

namespace PizzeriaOnline.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Pizza> PizzasDisponibles { get; set; }
        public List<Promocion> PromocionesActivas { get; set; }
    }
}
