using Microsoft.AspNetCore.Mvc;
using PostService.Data;
using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly DataAccess dataAccess;

        public PostController(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetLatestPost (string category, int count)
        {
            return await dataAccess.ReadLatestPosts(category, count);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            await dataAccess.CreatePost(post);

            return NoContent();
        }

        [HttpGet("InitDatabase")]
        public void InitDatabase([FromQuery] int countUsers, [FromQuery] int countCategories)
        {
            dataAccess.InitDatabase(countUsers, countCategories);
        }
    }
}
