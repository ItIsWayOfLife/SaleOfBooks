using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    public class CartBooksRepository : Repository, IRepository<CartBooks>
    {
        public CartBooksRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(CartBooks item)
        {
            var query = "INSERT INTO CartBooks(Count, CartId, BookId) output INSERTED.ID VALUES (@Count, @CartId, @BookId)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Count", item.Count);
            command.Parameters.AddWithValue("@CartId", item.CartId);
            command.Parameters.AddWithValue("@BookId", item.BookId);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM CartBooks WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<CartBooks> Find(Func<CartBooks, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public CartBooks Get(int id)
        {
            var command = CreateCommand("SELECT * FROM CartBooks WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new CartBooks
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Count = Convert.ToInt32(reader["Count"]),
                    CartId = Convert.ToInt32(reader["CartId"]),
                    BookId = Convert.ToInt32(reader["BookId"])
                };
            }
        }

        public IEnumerable<CartBooks> GetAll()
        {
            var cartsBooks = new List<CartBooks>();

            var command = CreateCommand("SELECT * FROM CartBooks WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cartsBooks.Add(new CartBooks()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Count = Convert.ToInt32(reader["Count"]),
                        CartId = Convert.ToInt32(reader["CartId"]),
                        BookId = Convert.ToInt32(reader["BookId"])
                    });
                }
            }

            return cartsBooks;
        }

        public void Update(CartBooks item)
        {
            var query = "UPDATE CartBooks SET Count = @Count, CartId = @CartId, BookId = @BookId WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Count", item.Count);
            command.Parameters.AddWithValue("@CartId", item.CartId);
            command.Parameters.AddWithValue("@BookId", item.BookId);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
