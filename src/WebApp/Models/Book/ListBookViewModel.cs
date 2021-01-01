using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebApp.Models.Book
{
    public class ListBookViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; }
        public SelectList ListSort { get; set; }
        public string SortString { get; set; }
        public SelectList ListGenres { get; set; }
        public string StringGenre { get; set; }
        public SelectList ListSearch { get; set; }
        public string SearchFor { get; set; }
        public string NameSearch { get; set; }
        public bool IsActive { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
