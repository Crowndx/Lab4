using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Lab4.Context;
using Lab4.Models;

namespace Lab4.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserMessageContext _userMessageContext;
        public ChatHub(UserMessageContext userMessageContext)
        {
            _userMessageContext = userMessageContext;
        }
        public async Task SendMessage(string user, string message)
        {           
            var dateTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");
            await Clients.All.SendAsync("ReceiveMessage", user, message, dateTime);
        }
        public async Task OnConnection(string user)
        {
            var dateTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string message = "connected";
            await Clients.All.SendAsync("ConnectionMessage",user,message, dateTime);
        }
        public async Task OnDisconnect(string user)
        {
            var dateTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string message = "disconnected";
            await Clients.All.SendAsync("DisconnectionMessage", user,message, dateTime);
        }
    }
}
