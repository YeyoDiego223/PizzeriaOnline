using PizzeriaOnline.Models;
using System.Collections.Generic;

namespace PizzeriaOnline.ViewModels
{
    public class SaborSeleccionado
    {
        public int PizzaId { get; set; }
        public decimal Porcion { get; set; }
    }

    public class ConstructorViewModel
    {        

        public List<Tamaño> TamañosDisponibles {  get; set; }
        public List<Pizza> SaboresDisponibles { get; set; }
        public int TamañoId { get; set; }
        public List<SaborSeleccionado> Sabores { get; set; } = new();        
    }
}
