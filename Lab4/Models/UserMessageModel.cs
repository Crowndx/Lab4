using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    /// <summary>
    /// Represents a message to the chat
    /// </summary>
    public class UserMessageModel
    {
        // Auto generated in database
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        // Provided by the client can only be upto 10 characters long
        [Required]
        [StringLength(10)]
        public string UserName { get; set; }

        // Provided by the client or server can only be upto 140 characters long
        [Required]
        [StringLength(140)]
        public string UserMessage { get; set; }

        // What time the message was sent at
        [Required]
        public DateTimeOffset Sent { get; set; }

        /// <summary>
        /// Generates the time the message was sent
        /// </summary>
        public UserMessageModel()
        {
            this.Sent = DateTimeOffset.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public UserMessageModel(string userName, string message) : this()
        {
            this.UserName = userName;
            this.UserMessage = message;
        }
    }
}
