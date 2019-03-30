using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class UserMessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string UserName { get; set; }
        [Required]
        [StringLength(140)]
        public string UserMessage { get; set; }
        [Required]
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
