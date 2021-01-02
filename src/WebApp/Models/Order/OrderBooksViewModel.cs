
namespace WebApp.Models.Order
{
    public class OrderBooksViewModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Path { get; set; }
        public string Author { get; set; }
    }
}
