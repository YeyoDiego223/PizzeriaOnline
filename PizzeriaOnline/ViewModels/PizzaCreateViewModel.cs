﻿// ViewModels/PizzaCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.ViewModels
{
    public class PizzaCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una imagen.")]
        public IFormFile ImagenArchivo { get; set; }
    }
}