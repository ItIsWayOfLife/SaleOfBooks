using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Core.Converters
{
    public class ConverterGenre : IConverter<Genre, GenreDTO>
    {
        private IUnitOfWork _Database { get; set; }

        private IEnumerable<Book> _books;

        public ConverterGenre(IUnitOfWork Database)
        {
            _Database = Database;
            _books = _Database.Book.GetAll();
        }

        public Genre ConvertDTOByModel(GenreDTO modelDTO)
        {
            Genre genre = new Genre()
            {
                Id = modelDTO.Id,
                Name = modelDTO.Name
            };

            return genre;
        }

        public IEnumerable<Genre> ConvertDTOsByModels(IEnumerable<GenreDTO> modelDTOs)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GenreDTO, Genre>()).CreateMapper();
            var genres = mapper.Map<IEnumerable<GenreDTO>, List<Genre>>(modelDTOs);

            return genres;
        }

        public IEnumerable<GenreDTO> ConvertModelsByDTOs(IEnumerable<Genre> models)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Genre, GenreDTO>()).CreateMapper();
            var genreDTOs = mapper.Map<IEnumerable<Genre>, List<GenreDTO>>(models);

            foreach (var gDTO in genreDTOs)
            {
                gDTO.CountBook = _books.Where(p => p.GenreId == gDTO.Id).Count();
            }

            return genreDTOs;
        }

        public GenreDTO ConvertModelByDTO(Genre model)
        {
            GenreDTO genreDTO = new GenreDTO()
            {
                Id = model.Id,
                Name = model.Name,
                CountBook = _books.Where(p => p.GenreId == model.Id).Count()
            };

            return genreDTO;
        }
    }
}
