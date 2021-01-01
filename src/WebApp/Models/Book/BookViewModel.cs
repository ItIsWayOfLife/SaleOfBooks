using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Book
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        [Required(ErrorMessage = "Enter name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter information")]
        [Display(Name = "Information")]
        public string Info { get; set; }

        [Required(ErrorMessage = "Enter the code")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Enter price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Picture")]
        public string Path { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Publisher")]
        public string PublishingHouse { get; set; }

        [Display(Name = "Year of writing")]
        public string YearOfWriting { get; set; }

        [Display(Name = "Year of publishing")]
        public string YearPublishing { get; set; }

        [Display(Name = "To the main")]
        public bool IsFavorite { get; set; }

        [Required(ErrorMessage = "Select genre")]
        [Display(Name = "Genre")]
        public string Genre { get; set; }

        [Display(Name = "Display")]
        public bool IsDisplay { get; set; }

        [Display(Name ="Is new?")]
        public bool IsNew { get; set; }
    }
}
