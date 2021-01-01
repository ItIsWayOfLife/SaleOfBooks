using Core.Constants;
using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class BookService: IBookService
    {
        private IUnitOfWork Database { get; set; }

        private readonly IConverter<Book, BookDTO> _converterBook;

        public BookService(IUnitOfWork uow,
            IConverter<Book, BookDTO> converterBook)
        {
            Database = uow;
            _converterBook = converterBook;
        }

        public BookDTO GetBook(int? id)
        {
            if (id == null)
                throw new ValidationException("Id book not found", "");

            var book = Database.Book.Get(id.Value);

            if (book == null)
                throw new ValidationException($"Book with id {id} not found", "");

            return _converterBook.ConvertModelByDTO(book);
        }

        public IEnumerable<BookDTO> GetBooks()
        {
            var booksDTOs = Database.Book.GetAll();
            return _converterBook.ConvertModelsByDTOs(booksDTOs);
        }

        public IEnumerable<BookDTO> GetFavoriteBooks()
        {
            return GetBooks().Where(p => p.IsFavorite == true);
        }

        public void Add(BookDTO bookDTO)
        {
            Database.Book.Create(_converterBook.ConvertDTOByModel(bookDTO));
            Database.Save();
        }

        public void Edit(BookDTO bookDTO)
        {
            Book book = Database.Book.Get(bookDTO.Id);

            if (book == null)
                throw new ValidationException("Book not found", "");

            book.Info = bookDTO.Info;
            book.IsDisplay = bookDTO.IsDisplay;
            book.IsFavorite = bookDTO.IsFavorite;
            book.IsNew = bookDTO.IsNew;
            book.Name = bookDTO.Name;
            book.Path = (bookDTO.Path).Replace(PathConstants.PAPH_BOOKS, "");
            book.Price = bookDTO.Price;
            book.PublishingHouse = bookDTO.PublishingHouse;
            book.YearOfWriting = bookDTO.YearOfWriting;
            book.YearPublishing = bookDTO.YearPublishing;
            book.Author = bookDTO.Author;
            book.Code = bookDTO.Code;
            book.GenreId = bookDTO.GenreId;
            
            Database.Book.Update(book);
            Database.Save();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new ValidationException("Id book not found", "");

            var book = Database.Book.Get(id.Value);

            if (book == null)
                throw new ValidationException($"Book with id {id} not found", "");

            Database.Book.Delete(id.Value);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
