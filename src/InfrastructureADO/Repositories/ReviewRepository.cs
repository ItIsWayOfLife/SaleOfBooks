using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    public class ReviewRepository : Repository, IRepository<Review>
    {
        public ReviewRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(Review item)
        {
            var query = "INSERT INTO Reviews(ApplicationUserId, Content, DateTime) output INSERTED.ID VALUES (@ApplicationUserId, @Content, @DateTime)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@ApplicationUserId", item.ApplicationUserId);
            command.Parameters.AddWithValue("@Content", item.Content);
            command.Parameters.AddWithValue("@DateTime", item.DateTime);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM Reviews WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<Review> Find(Func<Review, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Review Get(int id)
        {
            var command = CreateCommand("SELECT * FROM Reviews WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Review
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ApplicationUserId = reader["ApplicationUserId"].ToString(),
                    Content = reader["Content"].ToString(),
                    DateTime = Convert.ToDateTime(reader["DateTime"])
                };
            }
        }

        public IEnumerable<Review> GetAll()
        {
            var reviews = new List<Review>();

            var command = CreateCommand("SELECT * FROM Reviews WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    reviews.Add(new Review()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ApplicationUserId = reader["ApplicationUserId"].ToString(),
                        Content = reader["Content"].ToString(),
                        DateTime = Convert.ToDateTime(reader["DateTime"])
                    });
                }
            }

            return reviews;
        }

        public void Update(Review item)
        {
            var query = "UPDATE Reviews SET ApplicationUserId = @ApplicationUserId, Content = @Content, DateTime = @DateTime WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@ApplicationUserId", item.ApplicationUserId);
            command.Parameters.AddWithValue("@Content", item.Content);
            command.Parameters.AddWithValue("@DateTime", item.DateTime);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
