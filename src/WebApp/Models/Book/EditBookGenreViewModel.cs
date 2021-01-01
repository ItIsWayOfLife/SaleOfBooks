using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Book
{
    public class EditBookGenreViewModel
    {
        public BookViewModel BookViewModel { get; set; }
        public SelectList Genres { get; set; }
    }
}
