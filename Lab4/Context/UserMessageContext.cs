using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Context
{
    public class UserMessageContext : DbContext
    {
        public UserMessageContext(DbContextOptions<UserMessageContext> options) : base(options)
        {

        }
        public DbSet<UserMessageModel> Message { get; set; }
    }
}
