using AutoMapper;
using Core.Constants;
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;

namespace Core.Converters
{
    public class ConverterBook : IConverter<Book, BookDTO>
    {
        private IGenreService _genreService;

        public ConverterBook(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public Book ConvertDTOByModel(BookDTO modelDTO)
        {
            Book book = new Book()
            {
                Id = modelDTO.Id,
                Name = modelDTO.Name,
                Author = modelDTO.Author,
                Code = modelDTO.Code,
                GenreId = modelDTO.GenreId,
                Info = modelDTO.Info,
                IsDisplay = modelDTO.IsDisplay,
                IsFavorite = modelDTO.IsFavorite,
                IsNew = modelDTO.IsNew,
                Path = (modelDTO.Path).Replace(PathConstants.PARH_BOOKS, ""),
                Price = modelDTO.Price,
                PublishingHouse = modelDTO.PublishingHouse,
                YearPublishing = modelDTO.YearPublishing
            };

            return book;
        }

        public IEnumerable<Book> ConvertDTOsByModels(IEnumerable<BookDTO> modelDTOs)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, Book>()).CreateMapper();
            var books = mapper.Map<IEnumerable<BookDTO>, List<Book>>(modelDTOs);

            return books;
        }

        public IEnumerable<BookDTO> ConvertModelsByDTOs(IEnumerable<Book> models)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDTO>()).CreateMapper();
            var bookDTOs = mapper.Map<IEnumerable<Book>, List<BookDTO>>(models);

            foreach (var bookDTO in bookDTOs)
            {
                bookDTO.Path = PathConstants.PARH_BOOKS + bookDTO.Path;
                bookDTO.Genre = _genreService.GetGenre(bookDTO.GenreId).Name;
            }

            return bookDTOs;
        }

        public BookDTO ConvertModelByDTO(Book model)
        {
            BookDTO bookDTO = new BookDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Author = model.Author,
                YearPublishing = model.YearPublishing,
                PublishingHouse = model.PublishingHouse,
                Price = model.Price,
                Path = PathConstants.PARH_BOOKS + model.Path,
                IsNew = model.IsNew,
                Code = model.Code,
                Genre = _genreService.GetGenre(model.GenreId).Name,
                GenreId = model.GenreId,
                Info = model.Info,
                IsDisplay = model.IsDisplay,
                IsFavorite = model.IsFavorite
            };

            return bookDTO;
        }      
    }
}
