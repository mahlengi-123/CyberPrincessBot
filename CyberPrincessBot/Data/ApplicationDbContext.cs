using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CyberSafeBot.Models;


namespace CyberSafeBot.Data
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<CyberSafeBot.Models.Task> Tasks { get; set; }
        public DbSet<CyberSafeBot.Models.Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
        }
    }
}


    

