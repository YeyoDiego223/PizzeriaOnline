using PizzeriaOnline.Models;
using System.Collections.Generic;

namespace PizzeriaOnline.ViewModels
{
    public class GestionarRecetaViewModel
    {
        public int PizzaId { get; set; }
        public string PizzaNombre { get; set; }
        public int TamañoId { get; set; }
        public string TamañoNombre { get; set; }

        // Esta lista DEBE ser del tipo IngredienteRecetaViewModel
        public List<IngredienteRecetaViewModel> Ingredientes { get; set; }
    }

    // Esta es la clase auxiliar correcta que debe usarse
    public class IngredienteRecetaViewModel
    {
        public int IngredienteId { get; set; }
        public string Nombre { get; set; }
        public bool Asignado { get; set; }
        public decimal Cantidad { get; set; }
        public string UnidadDeMedida { get; set; }
    }
}
