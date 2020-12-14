using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    class ReviewRepository : IRepository<Review>
    {
        private readonly ApplicationContext _applicationContext;

        public ReviewRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(Review item)
        {
            _applicationContext.Reviews.Add(item);
        }

        public void Delete(int id)
        {
            Review review = _applicationContext.Reviews.Find(id);
            if (review != null)
            {
                _applicationContext.Reviews.Remove(review);
            }
        }

        public IEnumerable<Review> Find(Func<Review, bool> predicate)
        {
            return _applicationContext.Reviews.Where(predicate).ToList();
        }

        public Review Get(int id)
        {
            return _applicationContext.Reviews.Find(id);
        }

        public IEnumerable<Review> GetAll()
        {
            return _applicationContext.Reviews;
        }

        public void Update(Review item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
