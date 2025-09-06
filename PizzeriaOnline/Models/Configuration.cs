using System.ComponentModel.DataAnnotations;

namespace PizzeriaOnline.Models
{
    public class Configuracion
    {
        public int Id { get; set; }
        [Display(Name = "Forzar Cierre (Anula el horario)")]
        public bool ForzarCierre { get; set; } = false;

        [Required]
        [Display(Name = "Hora de Apertura")]
        public TimeSpan HoraApertura {  get; set; }

        [Required]
        [Display(Name = "Hora de Cierre")]
        public TimeSpan HoraCierre { get; set; }
    }
}