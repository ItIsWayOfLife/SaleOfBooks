using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public string Content { get; set; }
        public List<Like> Likes { get; set; }
        public DateTime DateTime { get; set; }
    }
}
