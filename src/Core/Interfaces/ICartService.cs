using Core.DTO;
using Core.Entities;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface ICartService
    {
        Cart Create(string applicationUserId);

        CartDTO GetCart(string applicationUserId);

        IEnumerable<CartBooksDTO> GetCartBooks(string applicationUserId);

        void DeleteCartBook(int? id, string applicationUserId);

        void AddBookToCart(int? bookId, string applicationUserId);

        void AllDeleteBooksToCart(string applicationUserId);

        void UpdateCountBookInCart(string applicationUserId, int? bookCartId, int count);

        decimal FullPriceCart(string applicationUserId);

        int GetCount(string applicationUserId);
    }
}
