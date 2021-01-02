using System;

namespace WebApp.Models.Order
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public decimal FullPrice { get; set; }
        public int CountBook { get; set; }
    }
}
