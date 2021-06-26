using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data
{
    public class PostServiceContext : DbContext
    {
        private readonly string connectionString;

        public PostServiceContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }

        public DbSet<Entities.Post> Post { get; set; }
        public DbSet<Entities.User> User { get; set; }
        public DbSet<Entities.Category> Category { get; set; }
    }
}
