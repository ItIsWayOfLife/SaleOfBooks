﻿using System;

namespace Core.DTO
{
    public class FeedBackDTO
    {
        public int Id { get; set; }
        public string UserIdAsking { get; set; }
        public string UserIdAnswering { get; set; }
        public DateTime? DateTimeQuestion { get; set; }
        public string Question { get; set; }
        public DateTime? DateTimeAnswer { get; set; }
        public string Answer { get; set; }
        public bool IsAnswered { get; set; }
    }
}
