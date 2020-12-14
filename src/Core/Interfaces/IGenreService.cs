﻿using Core.DTO;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IGenreService
    {
        GenreDTO GetGenre(int? id);
        IEnumerable<GenreDTO> GetGenres();
        void Add(GenreDTO genreDTO);
        void Edit(GenreDTO genreDTO);
        void Delete(int? id);
        void Dispose();
    }
}
