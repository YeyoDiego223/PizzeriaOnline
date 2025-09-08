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

        [Display(Name = "Activar Promoción")]
        public bool PromocionEstaActiva { get; set; }

        [Display(Name = "Titulo de la Promocion")]
        public string? PromocionTitulo { get; set; }

        [Display(Name = "Descripcion de la Promoción")]
        public string? PromocionDescripcion { get; set; }

        [Display(Name = "Precio de la Promoción")]
        [Range(0, 10000)]
        public decimal? PromocionPrecio { get; set; }

        public string? PromocionRutaImagen { get; set; }
    }
}