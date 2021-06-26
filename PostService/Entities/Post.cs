using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities
{
    [Index(nameof(PostId), nameof(CategoryId))]
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
