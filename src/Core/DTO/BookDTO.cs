
namespace Core.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Path { get; set; }
        public string Author { get; set; }
        public string PublishingHouse { get; set; }
        public string YearPublishing { get; set; }
        public string YearOfWriting { get; set; }
        public bool IsNew { get; set; }
        public bool IsFavorite { get; set; }
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public bool IsDisplay { get; set; }
    }
}
