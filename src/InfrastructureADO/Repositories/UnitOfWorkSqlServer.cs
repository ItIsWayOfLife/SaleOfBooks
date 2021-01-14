using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace InfrastructureADO.Repositories
{
    public class UnitOfWorkSqlServer : IUnitOfWork
    {
        private readonly IConfiguration _configuration;

        private SqlConnection _context { get; set; }
        private SqlTransaction _transaction { get; set; }

        private CartRepository _cartRepository;
        private CartBooksRepository _cartBooksRepository;
        private OrderRepository _orderRepository;
        private OrderBooksRepository _orderBooksRepository;
        private BookRepository _bookRepository;
        private GenreReposotiry _genreReposotiry;
        private LikeReposotory _likeReposotory;
        private ReviewRepository _reviewRepository;
        private FeedBackRepository _feedBackRepository;

        public UnitOfWorkSqlServer(IConfiguration configuration)
        {
            _configuration = configuration;

            //try
            //{
                _context = new SqlConnection(_configuration.GetConnectionString("CatalogConnection"));
                _context.Open();

                _transaction = _context.BeginTransaction();
            //}
            //finally
            //{
            //    _context.Close();
            //}
        }

        public IRepository<Genre> Genre
        {
            get
            {
                if (_genreReposotiry == null)
                {
                    _genreReposotiry = new GenreReposotiry(_context, _transaction);
                }
                return _genreReposotiry;
            }
        }

        public IRepository<Cart> Cart
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_context, _transaction);
                }
                return _cartRepository;
            }
        }

        public IRepository<CartBooks> CartBooks
        {
            get
            {
                if (_cartBooksRepository == null)
                {
                    _cartBooksRepository = new CartBooksRepository(_context, _transaction);
                }
                return _cartBooksRepository;
            }
        }

        public IRepository<Order> Order
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context, _transaction);
                }
                return _orderRepository;
            }
        }

        public IRepository<OrderBooks> OrderBooks
        {
            get
            {
                if (_orderBooksRepository == null)
                {
                    _orderBooksRepository = new OrderBooksRepository(_context, _transaction);
                }
                return _orderBooksRepository;
            }
        }

        public IRepository<Review> Review
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_context, _transaction);
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
                    _likeReposotory = new LikeReposotory(_context, _transaction);
                }
                return _likeReposotory;
            }
        }

        public IRepository<Book> Book
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BookRepository(_context, _transaction);
                }
                return _bookRepository;
            }
        }

        public IRepository<FeedBack> FeedBack
        {
            get
            {
                if (_feedBackRepository == null)
                {
                    _feedBackRepository = new FeedBackRepository(_context, _transaction);
                }
                return _feedBackRepository;
            }
        }

        public virtual void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_context != null)
            {
                _context.Close();
                _context.Dispose();
            }

            _cartRepository = null;
            _cartBooksRepository = null;
            _orderRepository = null;
            _orderBooksRepository = null;
            _bookRepository = null;
            _genreReposotiry = null;
            _likeReposotory = null;
            _reviewRepository = null;
            _feedBackRepository = null;

            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _transaction.Commit();
        }
    }
}
