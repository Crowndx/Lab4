using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class UserMessageModel
    {
        public string UserName { get; set; }
        public string UserMessage { get; set; }
        public DateTimeOffset Sent { get; set; }
        public UserMessageModel()
        {            
        }
        public UserMessageModel(string userName, string message, DateTimeOffset dateTime) : this()
        {
            this.UserName = userName;
            this.UserMessage = message;
            this.Sent = dateTime;
        }
    }
}
