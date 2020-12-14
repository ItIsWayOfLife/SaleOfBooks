using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class GenreReposotiry : IRepository<Genre>
    {
        private readonly ApplicationContext _applicationContext;

        public GenreReposotiry(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(Genre item)
        {
            _applicationContext.Genres.Add(item);
        }

        public void Delete(int id)
        {
            Genre genre = _applicationContext.Genres.Find(id);
            if (genre != null)
            {
                _applicationContext.Genres.Remove(genre);
            }
        }

        public IEnumerable<Genre> Find(Func<Genre, bool> predicate)
        {
            return _applicationContext.Genres.Where(predicate).ToList();
        }

        public Genre Get(int id)
        {
            return _applicationContext.Genres.Find(id);
        }

        public IEnumerable<Genre> GetAll()
        {
            return _applicationContext.Genres;
        }

        public void Update(Genre item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
