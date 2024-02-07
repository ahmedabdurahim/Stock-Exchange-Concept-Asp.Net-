using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_project.Models
{
    public class ProjectContext:DbContext
    {
        public DbSet<User> Users {get; set;}
        public DbSet<IPO> IPO {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite(@"Data Source=database.db");
    }
}