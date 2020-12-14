using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;

namespace Infrastructure.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;

        private CartRepository _cartRepository;
        private CartBooksRepository _cartBooksRepository;
        private OrderRepository _orderRepository;
        private OrderBooksRepository _orderBooksRepository;
        private BookRepository _bookRepository;
        private GenreReposotiry _genreReposotiry;
        private LikeReposotory _likeReposotory;
        private ReviewRepository _reviewRepository;
        private FeedBackRepository _feedBackRepository;

        public EFUnitOfWork(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IRepository<FeedBack> FeedBack
        {
            get
            {
                if (_feedBackRepository == null)
                {
                    _feedBackRepository = new FeedBackRepository(_applicationContext);
                }
                return _feedBackRepository;
            }
        }

        public IRepository<Review> Review
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_applicationContext);
                }
                return _reviewRepository;
            }
        }

        public IRepository<Like> Like
        {
            get
            {
                if (_likeReposotory == null)
                {
                    _likeReposotory = new LikeReposotory(_applicationContext);
                }
                return _likeReposotory;
            }
        }

        public IRepository<CartBooks> CartBooks
        {
            get
            {
                if (_cartBooksRepository == null)
                {
                    _cartBooksRepository = new CartBooksRepository(_applicationContext);
                }
                return _cartBooksRepository;
            }
        }

        public IRepository<Cart> Cart
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_applicationContext);
                }
                return _cartRepository;
            }
        }

        public IRepository<Book> Book
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BookRepository(_applicationContext);
                }
                return _bookRepository;
            }
        }

        public IRepository<Genre> Genre
        {
            get
            {
                if (_genreReposotiry == null)
                {
                    _genreReposotiry = new GenreReposotiry(_applicationContext);
                }
                return _genreReposotiry;
            }
        }

        public IRepository<OrderBooks> OrderBooks
        {
            get
            {
                if (_orderBooksRepository == null)
                {
                    _orderBooksRepository = new OrderBooksRepository(_applicationContext);
                }
                return _orderBooksRepository;
            }
        }

        public IRepository<Order> Order
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_applicationContext);
                }
                return _orderRepository;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _applicationContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _applicationContext.SaveChanges();
        }
    }
}
