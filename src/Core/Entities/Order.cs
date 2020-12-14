using System;

namespace Core.Entities
{
    public class Order 
    {
        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
