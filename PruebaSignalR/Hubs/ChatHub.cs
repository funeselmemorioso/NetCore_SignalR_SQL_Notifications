using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task EnviarMensaje(String usuario, String mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", usuario, mensaje);
        }

    }
}
