using Core.DTO;
using System.Collections.Generic;
using WebApp.Models.Book;

namespace WebApp.Interfaces
{
    public interface IBookHelper
    {
        List<string> GetGenres();
        List<BookViewModel> GetBooksViewModel(IEnumerable<BookDTO> booksDtos);
        BookViewModel GetBookViewModel(BookDTO bookDto);
       int? GetGenreIdByName(string name);
    }
}
