using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class OrderBooksRepository : IRepository<OrderBooks>
    {
        private readonly ApplicationContext _applicationContext;

        public OrderBooksRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(OrderBooks item)
        {
            _applicationContext.OrderBooks.Add(item);
        }

        public void Delete(int id)
        {
            OrderBooks orderBooks = _applicationContext.OrderBooks.Find(id);
            if (orderBooks != null)
            {
                _applicationContext.OrderBooks.Remove(orderBooks);
            }
        }

        public IEnumerable<OrderBooks> Find(Func<OrderBooks, bool> predicate)
        {
            return _applicationContext.OrderBooks.Include(o => o.Order).Include(d => d.Book).Where(predicate).ToList();
        }

        public OrderBooks Get(int id)
        {
            return _applicationContext.OrderBooks.Find(id);
        }

        public IEnumerable<OrderBooks> GetAll()
        {
            return _applicationContext.OrderBooks.Include(o => o.Order).Include(d => d.Book);
        }

        public void Update(OrderBooks item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
