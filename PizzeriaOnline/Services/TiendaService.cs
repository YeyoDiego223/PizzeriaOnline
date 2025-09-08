using PizzeriaOnline.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore; // <-- Asegúrate de tener este 'using'

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
            // --- LA CORRECCIÓN ESTÁ AQUÍ ---
            // Añadimos AsNoTracking() para asegurar que siempre lea el dato más reciente
            var config = _context.Configuracion.AsNoTracking().FirstOrDefault();

            if (config == null)
            {
                return false;
            }

            if (config.ForzarCierre)
            {
                return false;
            }

            var horaActual = DateTime.Now.TimeOfDay;
            return horaActual >= config.HoraApertura && horaActual < config.HoraCierre;
        }
    }
}