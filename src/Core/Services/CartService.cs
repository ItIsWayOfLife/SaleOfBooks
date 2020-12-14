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
                throw new ValidationException("Не установлен id пользователя", "");

            Database.Cart.Create(new Cart() { ApplicationUserId = applicationUserId });
            Database.Save();

            return Database.Cart.Find(p => p.ApplicationUserId == applicationUserId).FirstOrDefault();
        }

        public CartDTO GetCart(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("Не установлен id пользователя", "");

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
                throw new ValidationException("Не установлен id пользователя", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Корзина не найдена", "");

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
                throw new ValidationException("Не установлен id пользователя", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Корзина не найдена", "");

            if (id == null)
                throw new ValidationException("Не установлено id удаляемой книги из корзины", "");

            var cartBook = Database.CartBooks.Get(id.Value);

            if (cartBook.CartId != cart.Id)
                throw new ValidationException("Унига в корзене не найдено", "");

            Database.CartBooks.Delete(id.Value);
            Database.Save();
        }

        public void AddBookToCart(int? bookId, string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("Не установлен id пользователя", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Корзина не найдена", "");

            if (bookId == null)
                throw new ValidationException("Не установлен id добавляемой книги в корзине", "");

            // если уже существует в корзине, то увеличивается на 1 (если нет, то созд нов с количеством 1)
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
                    throw new ValidationException("Книга не найдено", "");

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
                throw new ValidationException("Не установлен id пользователя", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Корзина не найдена", "");

            var cartBook = GetCartBooks(applicationUserId);

            if (cartBook.Count() < 1)
                throw new ValidationException("Корзина пуста", "");

            foreach (var cartD in cartBook)
            {
                Database.CartBooks.Delete(cartD.Id);
            }

            Database.Save();
        }

        public void UpdateCountBookInCart(string applicationUserId, int? bookCartId, int count)
        {
            if (applicationUserId == null)
                throw new ValidationException("Не установлен id пользователя", "");

            var cart = GetCart(applicationUserId);

            if (cart == null)
                throw new ValidationException("Корзина не найдена", "");

            var cartDishes = GetCartBooks(applicationUserId);

            if (cartDishes.Count() < 1)
                throw new ValidationException("Корзина пуста", "");

            if (cartDishes.Where(p => p.Id == bookCartId).Count() < 1)
                throw new ValidationException("В корзине нету указанной книги", "");

            CartBooks cartBooks = Database.CartBooks.Find(p => p.Id == bookCartId).FirstOrDefault();

            if (count <= 0)
                throw new ValidationException("Количество должно быть положительныим целым числом", "");

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
    }
}
