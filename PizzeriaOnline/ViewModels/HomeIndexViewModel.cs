using PizzeriaOnline.Models;

namespace PizzeriaOnline.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Pizza> PizzasDisponibles { get; set; }
        public Configuracion Configuracion { get; set; }       
    }
}
