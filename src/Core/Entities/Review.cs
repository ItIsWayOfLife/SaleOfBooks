using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string Content { get; set; }
        public List<Like> Likes { get; set; }
        public DateTime DateTime { get; set; }
    }
}
