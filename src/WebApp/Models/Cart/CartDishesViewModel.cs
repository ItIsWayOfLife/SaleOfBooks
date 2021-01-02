using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Cart
{
    public class CartDishesViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Amount")]
        [Range(1, 1000, ErrorMessage = "The number of books to order is limited")]
        public int Count { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Path { get; set; }
        public string Author { get; set; }
    }
}
