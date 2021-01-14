using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    public class GenreReposotiry : Repository, IRepository<Genre>
    {
        public GenreReposotiry(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(Genre item)
        {
            var query = "INSERT INTO Genres(Name) output INSERTED.ID VALUES (@Name)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Name", item.Name);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM Genres WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<Genre> Find(Func<Genre, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Genre Get(int id)
        {
            var command = CreateCommand("SELECT * FROM Genres WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Genre
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
        }

        public IEnumerable<Genre> GetAll()
        {
            var genres = new List<Genre>();

            var command = CreateCommand("SELECT * FROM Genres WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    genres.Add(new Genre()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    });
                }
            }

            return genres;
        }

        public void Update(Genre item)
        {
            var query = "UPDATE Genres SET Name = @Name WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
