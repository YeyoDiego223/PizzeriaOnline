using PizzeriaOnline.Data;
using System;
using System.Linq;

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
            var config = _context.Configuracion.FirstOrDefault();

            if (config == null)
            {
                return false;
            }

            if (config.ForzarCierre)
            {
                return true;
            }

            var horaActual = DateTime.Now.TimeOfDay;

            return horaActual >= config.HoraApertura && horaActual < config.HoraCierre;
        }
    }
}
