using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class CartService : ICartService
    {
        private IUnitOfWork Database { get; set; }

        public CartService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public Cart Create(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            Database.Cart.Create(new Cart() { ApplicationUserId = applicationUserId });
            Database.Save();

            return Database.Cart.Find(p => p.ApplicationUserId == applicationUserId).FirstOrDefault();
        }

        public CartDTO GetCart(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            var cart = Database.Cart.Find(p => p.ApplicationUserId == applicationUserId).FirstOrDefault();

            if (cart == null)
            {
                cart = Create(applicationUserId);
            }

            CartDTO cartDTO = new CartDTO()
            {
                ApplicationUserId = cart.ApplicationUserId,
                Id = cart.Id
            };

            return cartDTO;
        }

        public IEnumerable<CartBooksDTO> GetCartBooks(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Cart not found", "");

            var cartBooks = Database.CartBooks.Find(p => p.CartId == cart.Id);

            var cartBooksDTO = new List<CartBooksDTO>();

            foreach (var cartB in cartBooks)
            {
                cartBooksDTO.Add(new CartBooksDTO()
                {
                    CartId = cart.Id,
                    Id = cartB.Id,
                    BookId = cartB.BookId,
                    Code = cartB.Book.Code,
                    Count = cartB.Count,
                    Name = cartB.Book.Name,
                    Path = cartB.Book.Path,
                    Price = cartB.Book.Price
                });
            }

            return cartBooksDTO;
        }

        public void DeleteCartBook(int? id, string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Cart not found", "");

            if (id == null)
                throw new ValidationException("The id of the book to be deleted from the basket is not set", "");

            var cartBook = Database.CartBooks.Get(id.Value);

            if (cartBook.CartId != cart.Id)
                throw new ValidationException("Книга не найдена в корзине", "");

            Database.CartBooks.Delete(id.Value);
            Database.Save();
        }

        public void AddBookToCart(int? bookId, string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Cart not found", "");

            if (bookId == null)
                throw new ValidationException("The id of the added book in the cart is not set", "");

            // if it already exists in the basket, then it is increased by 1 (if not, then created with the number of 1)
            if (GetCartBooks(applicationUserId).Where(p => p.BookId == bookId.Value).Count() > 0)
            {
                var cartBook = Database.CartBooks.Find(p => p.CartId == cart.Id).Where(p => p.BookId == bookId.Value).FirstOrDefault();
                cartBook.Count++;
                Database.CartBooks.Update(cartBook);
                Database.Save();
            }
            else
            {
                Book book = Database.Book.Get(bookId.Value);

                if (book == null)
                    throw new ValidationException("Book not found", "");

                Database.CartBooks.Create(new CartBooks()
                {
                    CartId = cart.Id,
                    Count = 1,
                    BookId = bookId.Value
                });

                Database.Save();
            }
        }

        public void AllDeleteBooksToCart(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Cart not found", "");

            var cartBook = GetCartBooks(applicationUserId);

            if (cartBook.Count() < 1)
                throw new ValidationException("Cart is empty", "");

            foreach (var cartD in cartBook)
            {
                Database.CartBooks.Delete(cartD.Id);
            }

            Database.Save();
        }

        public void UpdateCountBookInCart(string applicationUserId, int? bookCartId, int count)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Cart not found", "");

            var cartDishes = GetCartBooks(applicationUserId);

            if (cartDishes.Count() < 1)
                throw new ValidationException("Cart is empty", "");

            if (cartDishes.Where(p => p.Id == bookCartId).Count() < 1)
                throw new ValidationException("The specified book is not in the basket", "");

            CartBooks cartBooks = Database.CartBooks.Find(p => p.Id == bookCartId).FirstOrDefault();

            if (count <= 0)
                throw new ValidationException("Quantity must be a positive integer", "");

            cartBooks.Count = count;

            Database.CartBooks.Update(cartBooks);
            Database.Save();
        }

        public decimal FullPriceCart(string applicationUserId)
        {
            decimal fullPrice = 0;

            var cartDishes = GetCartBooks(applicationUserId);

            foreach (var cartDish in cartDishes)
            {
                fullPrice += cartDish.Count * cartDish.Price;
            }

            return fullPrice;
        }

        public int GetCount(string applicationUserId)
        {
            int count = 0;

            var cartBooks = GetCartBooks(applicationUserId);

            foreach (var cartB in cartBooks)
            {
                count += cartB.Count;
            }

            return count;
        }
    }
}
