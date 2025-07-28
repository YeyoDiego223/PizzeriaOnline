using Microsoft.AspNetCore.SignalR;

namespace PizzeriaOnline.Hubs
{
    public class PedidoHub : Hub
    {

        public async Task UnirseAGrupoPedido(string pedidoId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, pedidoId);
        }
    }
}
