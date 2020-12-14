using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly ApplicationContext _applicationContext;

        public BookRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Create(Book item)
        {
            _applicationContext.Books.Add(item);
        }

        public void Delete(int id)
        {
            Book book = _applicationContext.Books.Find(id);
            if (book != null)
            {
                _applicationContext.Books.Remove(book);
            }
        }

        public IEnumerable<Book> Find(Func<Book, bool> predicate)
        {
            return _applicationContext.Books.Include(p => p.Genre).Where(predicate).ToList();
        }

        public Book Get(int id)
        {
            return _applicationContext.Books.Find(id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _applicationContext.Books.Include(p => p.Genre);
        }

        public void Update(Book item)
        {
            _applicationContext.Entry(item).State = EntityState.Modified;
        }
    }
}
