using PizzeriaOnline.Models;
using System.Collections.Generic;

namespace PizzeriaOnline.ViewModels
{
    public class AsignarIngredientesViewModel
    {
        public int PizzaId { get; set; }
        public string PizzaNombre { get; set; }

        // Esta será una lista de todos los ingredientes disponibles
        // con una casilla para saber si esta asignado o no
        public List<IngredienteAsignadoViewModel> Ingredientes { get; set; }

        public AsignarIngredientesViewModel()
        {
            Ingredientes = new List<IngredienteAsignadoViewModel>();
        }
    }

    // Una pequeña clase auxiliar para representar cada ingrediente en la lista
    public class IngredienteAsignadoViewModel
    {
        public int IngredienteId { get; set; }
        public string Nombre { get; set; }
        public bool Asignado { get; set; } // El checkbox
        public double Cantidad { get; set; }
    }
}
