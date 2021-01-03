using System;

namespace WebApp.Models.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string Content { get; set; }
        public int CountLikes { get; set; }
        public DateTime DateTime { get; set; }
        public string Path { get; set; }
        public string LFP { get; set; }
        public bool Like { get; set; }
    }
}
