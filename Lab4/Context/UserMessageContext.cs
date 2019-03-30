using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Context
{
    /// <summary>
    /// Database connection
    /// </summary>
    public class UserMessageContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public UserMessageContext(DbContextOptions<UserMessageContext> options) : base(options)
        {

        }
        /// <summary>
        /// The table the db will use
        /// </summary>
        public DbSet<UserMessageModel> Message { get; set; }
    }
}
