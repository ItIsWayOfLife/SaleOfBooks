using System;

namespace Core.DTO
{
    public class ReviewDTO 
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string Content { get; set; }
        public int CountLikes { get; set; }
        public DateTime DateTime { get; set; }
    }
}
