using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class LikeReposotory : IRepository<Like>
    {
        private readonly ApplicationContext _applicationContext;

        public LikeReposotory(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(Like item)
        {
            _applicationContext.Likes.Add(item);
        }

        public void Delete(int id)
        {
            Like like = _applicationContext.Likes.Find(id);
            if (like != null)
            {
                _applicationContext.Likes.Remove(like);
            }
        }

        public IEnumerable<Like> Find(Func<Like, bool> predicate)
        {
            return _applicationContext.Likes.Where(predicate).ToList();
        }

        public Like Get(int id)
        {
            return _applicationContext.Likes.Find(id);
        }

        public IEnumerable<Like> GetAll()
        {
            return _applicationContext.Likes;
        }

        public void Update(Like item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
