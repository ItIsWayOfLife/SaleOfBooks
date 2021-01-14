using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
  public  class OrderBooksRepository : Repository, IRepository<OrderBooks>
    {
        public OrderBooksRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(OrderBooks item)
        {
            var query = "INSERT INTO OrderBooks(Count, OrderId, BookId) output INSERTED.ID VALUES (@Count, @OrderId, @BookId)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Count", item.Count);
            command.Parameters.AddWithValue("@OrderId", item.OrderId);
            command.Parameters.AddWithValue("@BookId", item.BookId);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM OrderBooks WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<OrderBooks> Find(Func<OrderBooks, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public OrderBooks Get(int id)
        {
            var command = CreateCommand("SELECT * FROM OrderBooks WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new OrderBooks
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Count = Convert.ToInt32(reader["Count"]),
                    OrderId = Convert.ToInt32(reader["OrderId"]),
                    BookId = Convert.ToInt32(reader["BookId"])
                };
            }
        }

        public IEnumerable<OrderBooks> GetAll()
        {
            var ordersBooks = new List<OrderBooks>();

            var command = CreateCommand("SELECT * FROM OrderBooks WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ordersBooks.Add(new OrderBooks()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Count = Convert.ToInt32(reader["Count"]),
                        OrderId = Convert.ToInt32(reader["OrderId"]),
                        BookId = Convert.ToInt32(reader["BookId"])
                    });
                }
            }

            return ordersBooks;
        }

        public void Update(OrderBooks item)
        {
            var query = "UPDATE OrderBooks SET Count = @Count, OrderId = @OrderId, BookId = @BookId WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Count", item.Count);
            command.Parameters.AddWithValue("@OrderId", item.OrderId);
            command.Parameters.AddWithValue("@BookId", item.BookId);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
