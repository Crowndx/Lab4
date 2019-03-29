using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class UserMessageModel
    {
        //DB Table has an auto increment Id as the primary key
        public string UserName { get; set; }
        public string UserMessage { get; set; }
        public DateTimeOffset Sent { get; set; }
        public UserMessageModel()
        {
            this.Sent = DateTimeOffset.Now;
        }
        public UserMessageModel(string userName, string message) : this()
        {
            this.UserName = userName;
            this.UserMessage = message;
        }
    }
}
