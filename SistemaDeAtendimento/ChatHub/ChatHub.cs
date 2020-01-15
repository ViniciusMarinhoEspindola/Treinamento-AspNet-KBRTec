using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SistemaDeAtendimento.ChatHub
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string group = "Todos")
        {
            try
            {
                Clients.Group(group).addNewMessageToPage(name, message);
            } catch
            {
                Clients.Group("Todos").addNewMessageToPage(name, message);
            }
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} foi conectado ao grupo {groupName}.");
        }
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} saiu do grupo {groupName}.");
        }
    }
}