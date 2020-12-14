using Core.Entities;
using System;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Book> Book { get; }
        IRepository<Genre> Genre { get; }
        IRepository<Cart> Cart { get; }
        IRepository<CartBooks> CartBooks { get; }
        IRepository<Order> Order { get; }
        IRepository<OrderBooks> OrderBooks { get; }
        IRepository<Like> Like { get; }
        IRepository<Review> Review { get; }
        IRepository<FeedBack> FeedBack { get; }

        void Save();
    }
}
