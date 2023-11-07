using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Controllers
{
    //public class dbContent
    
    public class UserDbContext : DbContext
    {


        protected readonly IConfiguration Configuration;

        public UserDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public UserDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));


        }

        public DbSet<User> Users { get; set; }

    }
}


