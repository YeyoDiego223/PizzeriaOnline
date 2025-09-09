using PizzeriaOnline.Data;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaOnline.Services
{
    public class TiendaService
    {
        private readonly ApplicationDbContext _context;

        public TiendaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool EstaAbierta()
        {
            var config = _context.Configuracion.AsNoTracking().FirstOrDefault();
            if (config == null)
            {
                return false;
            }

            if (config.ForzarCierre)
            {
                return false;
            }

            try
            {
                // --- INICIO DE LA CORRECCIÓN DE ZONA HORARIA ---
                // 1. Definimos la zona horaria del centro de México
                TimeZoneInfo zonaHorariaMexico = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

                // 2. Obtenemos la hora actual en UTC y la convertimos a la hora de México
                DateTime horaMexico = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaHorariaMexico);

                // 3. Comparamos usando la hora correcta de México
                var horaActual = horaMexico.TimeOfDay;
                // --- FIN DE LA CORRECCIÓN ---

                return horaActual >= config.HoraApertura && horaActual < config.HoraCierre;
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback por si el servidor no encuentra la zona horaria (muy raro en Windows/Azure)
                return false;
            }
        }
    }
}