using System;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.Models
{
    public class MensajeChat
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; } // Para saber a que pedido pertenece

        [Required]
        public string Autor { get; set; } // "Admin" o "Cliente"

        [Required]
        public string Texto { get; set; }

        public DateTime FechaEnvio { get; set; }

        public MensajeChat()
        {
            FechaEnvio = DateTime.UtcNow;
        }
    }
}
