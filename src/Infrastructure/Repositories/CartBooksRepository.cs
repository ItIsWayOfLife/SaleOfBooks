using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CartBooksRepository : IRepository<CartBooks>
    {
        private readonly ApplicationContext _applicationContext;

        public CartBooksRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(CartBooks item)
        {
            _applicationContext.CartBooks.Add(item);
        }

        public void Delete(int id)
        {
            CartBooks cartBooks = _applicationContext.CartBooks.Find(id);
            if (cartBooks != null)
            {
                _applicationContext.CartBooks.Remove(cartBooks);
            }
        }

        public IEnumerable<CartBooks> Find(Func<CartBooks, bool> predicate)
        {
            return _applicationContext.CartBooks.Include(c => c.Cart).Include(d => d.Book).Where(predicate).ToList();
        }

        public CartBooks Get(int id)
        {
            return _applicationContext.CartBooks.Find(id);
        }

        public IEnumerable<CartBooks> GetAll()
        {
            return _applicationContext.CartBooks.Include(c => c.Cart).Include(d => d.Book);
        }

        public void Update(CartBooks item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
