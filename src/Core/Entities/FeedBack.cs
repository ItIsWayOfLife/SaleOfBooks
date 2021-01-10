using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class FeedBack
    {
        public int Id { get; set; }

        [Required]
        public string UserIdAsking { get; set; }
        public string UserIdAnswering { get; set; }
        public DateTime? DateTimeQuestion { get; set; }

        [Required]
        public string Question { get; set; }
        public DateTime? DateTimeAnswer { get; set; }
        public string Answer { get; set; }
        public bool IsAnswered { get; set; }
    }
}
