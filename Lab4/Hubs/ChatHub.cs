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
    /// <summary>
    /// Represents the chat application
    /// Inherits from Hub(Signalr)
    /// </summary>
    public class ChatHub : Hub
    {
        // Holds all the "Connected" users names
        private static List<string> currentUsers = new List<string>();

        // The EntityFrameWork Database context that connects to the Message table in my ChatMessages DB
        private readonly UserMessageContext _userMessageContext;
        
        /// <summary>
        /// Sets my Db context so the program can store messages in the database and retrieve all messages
        /// </summary>
        /// <param name="userMessageContext"></param>
        public ChatHub(UserMessageContext userMessageContext)
        {
            _userMessageContext = userMessageContext;
        }

        /// <summary>
        /// Responsible for when a user sends a message to the chat
        /// Checks to see if the user is "Connected" to the chat by checking to see if the username is in the currentUsers list
        /// If the user is in the currentUsers list the chat will send the users message to all users and store it in the DB context
        /// Else it will send a message to just the user that they must "log in"
        /// </summary>
        /// <param name="user">Holds the userName provided by the client</param>
        /// <param name="message">Holds the message sent by the client</param>
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
            // checks to see if the client's userName is already "connected" to the chat 
            if (currentUsers.Contains(user) == true)
            {
                // tells client that user is already "connected"
                await Clients.Caller.SendAsync("UniqueName");
            }
            // checks to see if the client's userName is blank
            else if (user.Trim() == "")
            {
                // tells client to enter a non blank name
                await Clients.Caller.SendAsync("BlankName");
            }
            else
            {
                // Clears the chat for the user "Connecting" to the chat so the chat is in order
                await Clients.Caller.SendAsync("ClearChat");

                // Retrieves all the messages in the database
                var messages = await _userMessageContext.Message.ToListAsync();
                foreach(var oneMessage in messages)
                {
                    // Displays all messages in the database to the client that is connecting
                    await Clients.Caller.SendAsync("Message", oneMessage.UserName, oneMessage.UserMessage, oneMessage.Sent);
                }
                // Adds the current user to the currentUsers list to keep track of what userNames are "connected" to the chat
                currentUsers.Add(user);
                // Server message 
                string message = "connected";
                // Creates the model to save in the database after its displayed to all clients
                UserMessageModel userMessage = new UserMessageModel(user, message);
                await SendMessageAllClientsAsync(userMessage);

                // Adds the new message to the database and saves the changes
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
            // Checks to see if the user is currently "connected" to the chat
            if (currentUsers.Contains(user))
            {
                // removes the user from the currentUsers list
                currentUsers.Remove(user);
                // sets server message to disconnected
                string message = "disconnected";
                // creates message object
                UserMessageModel userMessage = new UserMessageModel(user, message);
                // sends the server message to all users
                await SendMessageAllClientsAsync(userMessage);

                // adds the message to the database
                _userMessageContext.Message.Add(userMessage);
                // saves the changes to the database
                await _userMessageContext.SaveChangesAsync();
            }
            else
            {
                // sends message to client that called the method to tell them user is not "connected"
                await Clients.Caller.SendAsync("NotConnected");
            }
        }

        /// <summary>
        /// Method to eliminate code duplication in sending messages to clients
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        private async Task SendMessageAllClientsAsync(UserMessageModel userMessage)
        {
            // Sends the provided userMessage object to all clients
            await Clients.All.SendAsync("Message", userMessage.UserName.ToString(), 
                userMessage.UserMessage.ToString(), userMessage.Sent.ToString("yyyy-MM-dd hh:mm:ss"));
        }

    }
}
