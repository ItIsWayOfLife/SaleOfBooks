using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    public class CartRepository : Repository, IRepository<Cart>
    {
        public CartRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(Cart item)
        {
            var query = "INSERT INTO Carts(ApplicationUserId) output INSERTED.ID VALUES (@ApplicationUserId)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@ApplicationUserId", (object)item.ApplicationUserId ?? DBNull.Value);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM Carts WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<Cart> Find(Func<Cart, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Cart Get(int id)
        {
            var command = CreateCommand("SELECT * FROM Carts WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Cart
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ApplicationUserId = reader["ApplicationUserId"].ToString()
                };
            }
        }

        public IEnumerable<Cart> GetAll()
        {
            var carts = new List<Cart>();

            var command = CreateCommand("SELECT * FROM Carts WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    carts.Add(new Cart()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ApplicationUserId = reader["ApplicationUserId"].ToString()
                    });
                }
            }

            return carts;
        }

        public void Update(Cart item)
        {
            var query = "UPDATE Carts SET ApplicationUserId = @ApplicationUserId WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@ApplicationUserId", (object)item.ApplicationUserId ?? DBNull.Value);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
