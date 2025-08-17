using PizzeriaOnline.Models;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaOnline.ViewModels
{
    public class CarritoViewModel
    {
        public List<CarritoItem> Pizzas { get; set; } = new();
        public List<CarritoExtraViewModel> Extras { get; set; } = new();
        public decimal TotalGeneral { get; set; }
    }
}