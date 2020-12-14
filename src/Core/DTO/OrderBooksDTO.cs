
namespace Core.DTO
{
    public class OrderBooksDTO 
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Path { get; set; }
    }
}
