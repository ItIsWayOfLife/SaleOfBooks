using AutoMapper;
using Core.Constants;
using Core.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Models.Book;

namespace WebApp.Helper
{
    public class BookHelper : IBookHelper
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;

        public BookHelper(IBookService bookService,
            IGenreService genreService)        {
            _genreService = genreService;
            _bookService = bookService;
        }

        public List<string> GetGenres()
        {
            var genres = _genreService.GetGenres();

            List<string> listGenres = new List<string>() { "Genres"};

            foreach (var genre in genres)
            {
                listGenres.Add(genre.Name);
            }

            return listGenres;
        }

        public List<BookViewModel> GetBooksViewModel(IEnumerable<BookDTO> booksDtos)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BookDTO, BookViewModel>()).CreateMapper();
            var books = mapper.Map<IEnumerable<BookDTO>, List<BookViewModel>>(booksDtos);

            return books;
        }

        public BookViewModel GetBookViewModel(BookDTO bookDto)
        {
            BookViewModel bookViewModel = new BookViewModel()
            {
                Id = bookDto.Id,
                Genre = bookDto.Genre,
                GenreId = bookDto.GenreId,
                Name = bookDto.Name,
                Author = bookDto.Author,
                Code = bookDto.Code,
                Info = bookDto.Info,
                IsFavorite = bookDto.IsFavorite,
                Path = bookDto.Path,
                Price = bookDto.Price,
                PublishingHouse = bookDto.PublishingHouse,
                YearOfWriting = bookDto.YearOfWriting,
                YearPublishing = bookDto.PublishingHouse,
                IsDisplay = bookDto.IsDisplay,
                 IsNew = bookDto.IsNew
            };

            return bookViewModel;
        }

        public int? GetGenreIdByName(string name)
        {
            return _genreService.GetGenres()?.FirstOrDefault(p=>p.Name == name)?.Id;
        }
    }
}
