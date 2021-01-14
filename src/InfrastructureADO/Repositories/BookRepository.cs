using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    class BookRepository : Repository, IRepository<Book>
    {
        public BookRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(Book item)
        {
            var query = "INSERT INTO  Books(Name, Info, Code, Price, Path, Author, PublishingHouse, YearOfWriting, YearPublishing, IsFavorite, IsNew, GenreId, IsDisplay)" +
                " output INSERTED.ID VALUES (@Name, @Info, @Code, @Price, @Path, @Author, @PublishingHouse, @YearOfWriting, @YearPublishing, @IsFavorite, @IsNew, @GenreId, @IsDisplay)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Info", item.Info);
            command.Parameters.AddWithValue("@Code", item.Code);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@Path", (object)item.Path ?? DBNull.Value);
            command.Parameters.AddWithValue("@Author", (object)item.Author ?? DBNull.Value);
            command.Parameters.AddWithValue("@PublishingHouse", (object)item.PublishingHouse ?? DBNull.Value);
            command.Parameters.AddWithValue("@YearOfWriting", (object)item.YearOfWriting ?? DBNull.Value);
            command.Parameters.AddWithValue("@YearPublishing", (object)item.YearPublishing ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsFavorite", item.IsFavorite);
            command.Parameters.AddWithValue("@IsNew", item.IsNew);
            command.Parameters.AddWithValue("@GenreId", item.GenreId);
            command.Parameters.AddWithValue("@IsDisplay", item.IsDisplay);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM Books WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<Book> Find(Func<Book, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public Book Get(int id)
        {
            var command = CreateCommand("SELECT * FROM Books WITH(NOLOCK) WHERE id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Book
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Author = reader["Author"].ToString(),
                    Code = reader["Code"].ToString(),
                    GenreId = Convert.ToInt32(reader["GenreId"]),
                    Info = reader["Info"].ToString(),
                    IsDisplay = Convert.ToBoolean(reader["IsDisplay"]),
                    IsFavorite = Convert.ToBoolean(reader["IsFavorite"]),
                    IsNew = Convert.ToBoolean(reader["IsNew"]),
                    Path = reader["Path"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    PublishingHouse = reader["PublishingHouse"].ToString(),
                    YearOfWriting = reader["YearOfWriting"].ToString(),
                    YearPublishing = reader["YearPublishing"].ToString()
                };
            }
        }

        public IEnumerable<Book> GetAll()
        {
            List<Book> books = new List<Book>();

            var command = CreateCommand("SELECT * FROM Books WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    books.Add(new Book()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Author = reader["Author"].ToString(),
                        Code = reader["Code"].ToString(),
                        GenreId = Convert.ToInt32(reader["GenreId"]),
                        Info = reader["Info"].ToString(),
                        IsDisplay = Convert.ToBoolean(reader["IsDisplay"]),
                        IsFavorite = Convert.ToBoolean(reader["IsFavorite"]),
                        IsNew = Convert.ToBoolean(reader["IsNew"]),
                        Path = reader["Path"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        PublishingHouse = reader["PublishingHouse"].ToString(),
                        YearOfWriting = reader["YearOfWriting"].ToString(),
                        YearPublishing = reader["YearPublishing"].ToString()
                    });
                }
            }

            return books;
        }

        public void Update(Book item)
        {
            var query = "UPDATE Books SET Name = @Name, Info =@Info, Code =@Code, Price =@Price, Path =@Path, Author =@Author, PublishingHouse =@PublishingHouse," +
                " YearOfWriting =@YearOfWriting, YearPublishing =@YearPublishing, IsFavorite=@IsFavorite, IsNew=@IsNew, GenreId=@GenreId, IsDisplay=@IsDisplay WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@Name", item.Name);
            command.Parameters.AddWithValue("@Info", item.Info);
            command.Parameters.AddWithValue("@Code", item.Code);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@Path", (object)item.Path ?? DBNull.Value);
            command.Parameters.AddWithValue("@Author", (object)item.Author ?? DBNull.Value);
            command.Parameters.AddWithValue("@PublishingHouse", (object)item.PublishingHouse ?? DBNull.Value);
            command.Parameters.AddWithValue("@YearOfWriting", (object)item.YearOfWriting ?? DBNull.Value);
            command.Parameters.AddWithValue("@YearPublishing", (object)item.YearPublishing ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsFavorite", item.IsFavorite);
            command.Parameters.AddWithValue("@IsNew", item.IsNew);
            command.Parameters.AddWithValue("@GenreId", item.GenreId);
            command.Parameters.AddWithValue("@IsDisplay", item.IsDisplay);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
