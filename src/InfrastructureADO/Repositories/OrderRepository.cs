using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
   public class OrderRepository : Repository, IRepository<Order>
    {
        public OrderRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(Order item)
        {
            var query = "INSERT INTO Orders(DateOrder, ApplicationUserId) output INSERTED.ID VALUES (@DateOrder, @ApplicationUserId)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@DateOrder", item.DateOrder);
            command.Parameters.AddWithValue("@ApplicationUserId", (object)item.ApplicationUserId ?? DBNull.Value);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM Orders WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Order Get(int id)
        {
            var command = CreateCommand("SELECT * FROM Orders WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Order
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ApplicationUserId = reader["ApplicationUserId"].ToString(),
                    DateOrder = Convert.ToDateTime(reader["DateOrder"])
                };
            }
        }

        public IEnumerable<Order> GetAll()
        {
            var orders = new List<Order>();

            var command = CreateCommand("SELECT * FROM Orders WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    orders.Add(new Order()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ApplicationUserId = reader["ApplicationUserId"].ToString(),
                        DateOrder = Convert.ToDateTime(reader["DateOrder"])
                    });
                }
            }

            return orders;
        }

        public void Update(Order item)
        {
            var query = "UPDATE Orders SET ApplicationUserId = @ApplicationUserId, DateOrder = @DateOrder WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@ApplicationUserId", item.ApplicationUserId);
            command.Parameters.AddWithValue("@DateOrder", item.DateOrder);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
