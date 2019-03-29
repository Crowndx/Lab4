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
        // Supposed to hold all the users "currently" logged into the chat
        List<string> currentUsers = new List<string>();

        // Supposed to be my db connection context
        private readonly UserMessageContext _userMessageContext;
        public ChatHub(UserMessageContext userMessageContext)
        {
            _userMessageContext = userMessageContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HubMethodName("SendMessage")]
        public async Task SendMessageAsync(string user, string message)
        {
            // Commented this code out so the application will allow you to actually send messages for testing purposes
            //if (currentUsers.Contains(user))
            //{
                var dateTime = DateTimeOffset.Now;
                UserMessageModel userMessage = new UserMessageModel(user, message);
                await Clients.All.SendAsync("ReceiveMessage", user, message, dateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                // Supposed to add my messages to my db but does nothing
                _userMessageContext.Messages.Add(userMessage);
                await _userMessageContext.SaveChangesAsync();
            //}
            //else
            //{
                //await Clients.Caller.SendAsync("Login");
            //}       
        }

        /// <summary>
        /// Using this method for my "Connection" button to make testing easier
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HubMethodName("OnConnect")]
        public async Task OnConnectionAsync(string user)
        {
            if (currentUsers.Contains(user) == true)
            {
                await Clients.Caller.SendAsync("UniqueName");
            }
            else
            {
                //TODO Retrieve all previous messages in DB
                //TODO Output all previous messages to clients that "Connected"
                currentUsers.Add(user);
                var dateTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");
                string message = "connected";
                await Clients.All.SendAsync("ConnectionMessage", user, message, dateTime);
                //TODO Update DB with new connected message
            }
        }

        /// <summary>
        /// Using this method for my "Disconnect" button to make testing easier
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HubMethodName("OnDisconnect")]
        public async Task OnDisconnectAsync(string user)
        {
            currentUsers.Remove(user);
            var dateTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string message = "disconnected";
            await Clients.All.SendAsync("DisconnectionMessage", user, message, dateTime);
            //TODO UPDATE DB with new disconnected message
            //TODO CLEAR Webpage message list for client that "disconnected"
        }
    }
}
