using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class FeedBackRepository : IRepository<FeedBack>
    {
        private readonly ApplicationContext _applicationContext;

        public FeedBackRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(FeedBack item)
        {
            _applicationContext.FeedBacks.Add(item);
        }

        public void Delete(int id)
        {
            FeedBack feedBack = _applicationContext.FeedBacks.Find(id);
            if (feedBack != null)
            {
                _applicationContext.FeedBacks.Remove(feedBack);
            }
        }

        public IEnumerable<FeedBack> Find(Func<FeedBack, bool> predicate)
        {
            return _applicationContext.FeedBacks.Where(predicate).ToList();
        }

        public FeedBack Get(int id)
        {
            return _applicationContext.FeedBacks.Find(id);
        }

        public IEnumerable<FeedBack> GetAll()
        {
            return _applicationContext.FeedBacks;
        }

        public void Update(FeedBack item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
