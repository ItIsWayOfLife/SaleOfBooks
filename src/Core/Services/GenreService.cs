using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;

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
                throw new ValidationException("Id genre not found", "");

            var genre = Database.Genre.Get(id.Value);

            if (genre == null)
                throw new ValidationException($"Genre with id {id} not found", "");

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
                throw new ValidationException("Id genre not found", "");

            var genre = Database.Genre.Get(id.Value);

            if (genre == null)
                throw new ValidationException($"Genre with id {id} not found", "");

            Database.Genre.Delete(id.Value);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
