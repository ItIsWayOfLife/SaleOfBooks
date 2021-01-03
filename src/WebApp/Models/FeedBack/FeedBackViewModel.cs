using System;

namespace WebApp.Models.FeedBack
{
    public class FeedBackViewModel
    {
        public int Id { get; set; }
        public string UserIdAsking { get; set; }
        public string UserIdAnswering { get; set; }
        public string Question { get; set; }
        public DateTime? DateTimeQuestion { get; set; }
        public string Answer { get; set; }
        public DateTime? DateTimeAnswer { get; set; }
        public bool IsAnswered { get; set; }
        public string UserAskingLFP { get; set; }
        public string UserAskingPath { get; set; }
        public string UserAnsweringLFP { get; set; }
        public string UserAnsweringPath { get; set; }
    }
}
