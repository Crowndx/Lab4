using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Lab4.Context;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Hubs
{
    public class ChatHub : Hub
    {
        private static List<string> currentUsers = new List<string>();

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
            if (currentUsers.Contains(user))
            {
                UserMessageModel userMessage = new UserMessageModel(user, message);
                await SendMessageAllClientsAsync(userMessage);
               
                _userMessageContext.Message.Add(userMessage);
                await _userMessageContext.SaveChangesAsync();
            }
            else
            {
                await Clients.Caller.SendAsync("Login");
            }       
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
            else if (user.Trim() == "")
            {
                await Clients.Caller.SendAsync("BlankName");
            }
            else
            {
                var messages = await _userMessageContext.Message.ToListAsync();
                foreach(var oneMessage in messages)
                {
                    await Clients.Caller.SendAsync("Message", oneMessage.UserName, oneMessage.UserMessage, oneMessage.Sent);
                }

                currentUsers.Add(user);
                string message = "connected";
                UserMessageModel userMessage = new UserMessageModel(user, message);
                await SendMessageAllClientsAsync(userMessage);

                _userMessageContext.Message.Add(userMessage);
                await _userMessageContext.SaveChangesAsync();
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
            if (currentUsers.Contains(user))
            {
                currentUsers.Remove(user);
                string message = "disconnected";
                UserMessageModel userMessage = new UserMessageModel(user, message);
                await SendMessageAllClientsAsync(userMessage);

                _userMessageContext.Message.Add(userMessage);
                await _userMessageContext.SaveChangesAsync();
            }
            else
            {
                await Clients.Caller.SendAsync("NotConnected");
            }
        }

        private async Task SendMessageAllClientsAsync(UserMessageModel userMessage)
        {
            await Clients.All.SendAsync("Message", userMessage.UserName.ToString(), 
                userMessage.UserMessage.ToString(), userMessage.Sent.ToString("yyyy-MM-dd hh:mm:ss"));
        }

    }
}
