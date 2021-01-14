using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class GenreService : IGenreService
    {
        private IUnitOfWork Database { get; set; }

        private readonly IConverter<Genre, GenreDTO> _converterGenre;

        public GenreService(IUnitOfWork uow,
            IConverter<Genre, GenreDTO> converterGenre)
        {
            Database = uow;
            _converterGenre = converterGenre;
        }

        public IEnumerable<GenreDTO> GetGenres()
        {
            var genreDTOs = Database.Genre.GetAll();
            return _converterGenre.ConvertModelsByDTOs(genreDTOs);
        }

        public GenreDTO GetGenre(int? id)
        {
            if (id == null)
                throw new ValidationException("Id genre not found", string.Empty);

            var genre = Database.Genre.Get(id.Value);

            if (genre == null)
                throw new ValidationException($"Genre with id {id} not found", string.Empty);

            return _converterGenre.ConvertModelByDTO(genre);
        }

        public void Add(GenreDTO genreDTO)
        {
            Database.Genre.Create(_converterGenre.ConvertDTOByModel(genreDTO));
            Database.Save();
        }

        public void Edit(GenreDTO genreDTO)
        {
            Database.Genre.Update(_converterGenre.ConvertDTOByModel(genreDTO));
            Database.Save();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new ValidationException("Id genre not found", string.Empty);

            var genre = Database.Genre.Get(id.Value);

            if (genre == null)
                throw new ValidationException($"Genre with id {id} not found", string.Empty);

            Database.Genre.Delete(id.Value);
            Database.Save();
        }

        public IEnumerable<GenreDTO> GetGenresWithCountBooks()
        {
            var listGenres = Database.Genre.GetAll().ToList();
            var listBooks = Database.Book.GetAll().ToList();

            List<GenreDTO> listGenreDto = new List<GenreDTO>();

            foreach (var genre in listGenres)
            {
                listGenreDto.Add(new GenreDTO()
                {
                    CountBook = GetCountBooks(genre.Id, listBooks),
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            return listGenreDto;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private int GetCountBooks(int genreId, IEnumerable<Book> books)
        {
            int count = 0;

            foreach (var book in books.Where(p => p.IsDisplay == true))
            {
                if (book.GenreId == genreId)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
