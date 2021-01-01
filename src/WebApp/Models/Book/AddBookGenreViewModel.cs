using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Book
{
    public class AddBookGenreViewModel
    {
        public AddBookViewModel AddBookViewModel { get; set; }
        public SelectList Genres { get; set; }
    }
}
