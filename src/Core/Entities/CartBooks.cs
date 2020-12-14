
namespace Core.Entities
{
    public class CartBooks
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
