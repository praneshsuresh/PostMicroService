using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PostService.Data
{
    public class DataAccess
    {
        private readonly List<string> connectionStrings = new List<string>();

        public DataAccess(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetSection("PostDbConnectionStrings").GetChildren();
            foreach(var connectionString in connectionStrings)
            {
                Console.WriteLine("connectionString: " + connectionString.Value);
                this.connectionStrings.Add(connectionString.Value);
            }
        }

        //Method to read the latest posts
        public async Task<ActionResult<IEnumerable<Post>>> ReadLatestPosts(string category, int count)
        {
            using var dbContext = new PostServiceContext(GetConnectionString(category));

            return await dbContext.Post
                .OrderByDescending(p => p.PostId)
                .Take(count)
                .Include(x => x.User)
                .Where(p => p.CategoryId == category)
                .ToListAsync();
        }

        //Method to Create post
        public async Task<int> CreatePost(Post post)
        {
            using var dbContext = new PostServiceContext(GetConnectionString(post.CategoryId));
            dbContext.Post.Add(post);
            return await dbContext.SaveChangesAsync();

        }

        // Initializes database - deletes all tables in all shards and recreates them with data based on input parameters
        public void InitDatabase(int countUsers, int countCategories)
        {
            foreach (var connectionString in connectionStrings)
            {
                using var dbContext = new PostServiceContext(connectionString);
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                for (int i = 1; i <= countUsers; i++)
                {
                    dbContext.User.Add(
                        new User
                        {
                            Name = "User" + i,
                            Version = 1
                        });
                    dbContext.SaveChanges();
                }

                for (int i = 1; i <= countCategories; i++)
                {
                    dbContext.Category.Add(
                        new Category
                        {
                            CategoryId = "Category"+i
                        });
                    dbContext.SaveChanges();
                }

            }
        }

        // Get connection string method
        private string GetConnectionString(string category)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(category));
            var x = BitConverter.ToUInt16(hash, 0) % connectionStrings.Count;
            return connectionStrings[x];
        }
    }
}
