using Core.DTO;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IBookService
    {
        BookDTO GetBook(int? id);
        IEnumerable<BookDTO> GetBooks();
        public IEnumerable<BookDTO> GetFavoriteBooks();
        void Add(BookDTO bookDTO);
        void Edit(BookDTO bookDTO);
        void Delete(int? id);
        void Dispose();
    }
}
