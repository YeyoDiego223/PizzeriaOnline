using PizzeriaOnline.Models;
using System.Collections.Generic;

namespace PizzeriaOnline.ViewModels
{
    public class ConstructorViewModel
    {
        public List<Tamaño> TamañosDisponibles {  get; set; }
        public List<Pizza> SaboresDisponibles { get; set; }
        public  ConstructorViewModel() 
        {
            TamañosDisponibles = new List<Tamaño>();
            SaboresDisponibles = new List<Pizza>();
        }
    }
}
