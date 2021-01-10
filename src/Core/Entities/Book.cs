using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Book 
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Path { get; set; }
        public string Author { get; set; }
        public string PublishingHouse { get; set; }
        public string YearOfWriting { get; set; }
        public string YearPublishing{ get; set; }
        public bool IsFavorite { get; set; }
        public bool IsNew{ get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public bool IsDisplay { get; set; }
    }
}
