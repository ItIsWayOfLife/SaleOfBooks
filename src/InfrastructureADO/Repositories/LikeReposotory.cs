using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    public class LikeReposotory : Repository, IRepository<Like>
    {
        public LikeReposotory(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(Like item)
        {
            var query = "INSERT INTO Likes(UserId, ReviewId) output INSERTED.ID VALUES (@UserId, @ReviewId)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@UserId", (object)item.UserId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReviewId", item.ReviewId);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM Likes WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<Like> Find(Func<Like, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Like Get(int id)
        {
            var command = CreateCommand("SELECT * FROM Likes WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Like
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserId = reader["UserId"].ToString(),
                    ReviewId = Convert.ToInt32(reader["ReviewId"])
                };
            }
        }

        public IEnumerable<Like> GetAll()
        {
            var likes = new List<Like>();

            var command = CreateCommand("SELECT * FROM Likes WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    likes.Add(new Like()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UserId = reader["UserId"].ToString(),
                        ReviewId = Convert.ToInt32(reader["ReviewId"])
                    });
                }
            }

            return likes;
        }

        public void Update(Like item)
        {
            var query = "UPDATE Likes SET UserId = @UserId, ReviewId = @ReviewId WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@UserId", (object)item.UserId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReviewId", item.ReviewId);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
