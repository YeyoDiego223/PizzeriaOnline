using System;
using System.Collections.Generic;

namespace PizzeriaOnline.Models
{
    public class CarritoItem
    {
        public Guid Id { get; set; }
        public int TamañoId { get; set; }
        public string NombreTamaño { get; set; }
        public decimal PrecioFinal { get; set; }
        public int Cantidad {  get; set; }
        public List<string> NombresSabores { get; set; }

        public CarritoItem() 
        {
            Id = Guid.NewGuid();
            NombresSabores = new List<string>();
        }
    }
}
