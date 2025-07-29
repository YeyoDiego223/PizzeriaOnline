using Microsoft.AspNetCore.SignalR;
using PizzeriaOnline.Data;
using PizzeriaOnline.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaOnline.Hubs
{
    public class PedidoHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public PedidoHub (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UnirseAGrupoPedido(string pedidoId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, pedidoId);

            // 3. Cargar el historial del chat para el usuario que se acaba de conectar
            var historial = _context.MensajesChat
                .Where(m => m.PedidoId == int.Parse(pedidoId))
                .OrderBy(m => m.FechaEnvio)
                .ToList();
            // Enviamos el historial solo al cliente que llamo este método
            await Clients.Caller.SendAsync("CargarHistorial", historial);
        }       

        public async Task EnviarMensaje(string pedidoId, string usuario, string mensaje)
        {
            // 1. Guardar el mensaje en la base de datos
            var mensajeChat = new MensajeChat
            {
                PedidoId = int.Parse(pedidoId),
                Autor = usuario,
                Texto = mensaje
            };
            _context.MensajesChat.Add(mensajeChat);
            await _context.SaveChangesAsync();

            // Esto envía el mensaje a toso los que estén en el "canal" de este pedido.
            await Clients.Group(pedidoId).SendAsync("RecibirMensaje", usuario, mensaje);
        }
            
    }
}
