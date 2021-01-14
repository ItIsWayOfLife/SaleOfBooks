using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InfrastructureADO.Repositories
{
    public class FeedBackRepository : Repository, IRepository<FeedBack>
    {
        public FeedBackRepository(SqlConnection context, SqlTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public void Create(FeedBack item)
        {
            var query = "INSERT INTO FeedBacks(UserIdAsking, UserIdAnswering, DateTimeQuestion, Question, DateTimeAnswer, Answer, IsAnswered) " +
                "output INSERTED.ID VALUES (@UserIdAsking, @UserIdAnswering, @DateTimeQuestion, @Question, @DateTimeAnswer, @Answer, @IsAnswered)";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@UserIdAsking", item.UserIdAsking);
            command.Parameters.AddWithValue("@UserIdAnswering", (object)item.UserIdAnswering ?? DBNull.Value);
            command.Parameters.AddWithValue("@DateTimeQuestion", (object)item.DateTimeQuestion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Question", item.Question);
            command.Parameters.AddWithValue("@DateTimeAnswer", (object)item.DateTimeAnswer ?? DBNull.Value);
            command.Parameters.AddWithValue("@Answer", (object)item.Answer ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsAnswered", item.IsAnswered);

            item.Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(int id)
        {
            var command = CreateCommand("DELETE FROM FeedBacks WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public IEnumerable<FeedBack> Find(Func<FeedBack, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public FeedBack Get(int id)
        {
            var command = CreateCommand("SELECT * FROM FeedBacks WITH(NOLOCK) WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                DateTime dateTimeQuestion = new DateTime();
                DateTime dateTimeAnswer = new DateTime();

                if (!(reader["DateTimeAnswer"] is DBNull))
                    dateTimeAnswer = (Convert.ToDateTime(reader["DateTimeAnswer"]));

                if (!(reader["DateTimeQuestion"] is DBNull))
                    dateTimeQuestion = (Convert.ToDateTime(reader["DateTimeQuestion"]));

                return new FeedBack
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserIdAsking = reader["UserIdAsking"].ToString(),
                    UserIdAnswering = reader["UserIdAnswering"].ToString(),
                    DateTimeQuestion = dateTimeQuestion,
                    Question = reader["Question"].ToString(),
                    DateTimeAnswer = dateTimeAnswer,
                    Answer = reader["Answer"].ToString(),
                    IsAnswered = Convert.ToBoolean(reader["IsAnswered"])
                };
            }
        }

        public IEnumerable<FeedBack> GetAll()
        {
            var feedBacks = new List<FeedBack>();

            var command = CreateCommand("SELECT * FROM FeedBacks WITH(NOLOCK)");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime dateTimeQuestion = new DateTime();
                    DateTime dateTimeAnswer = new DateTime();

                    if (!(reader["DateTimeAnswer"] is DBNull))
                        dateTimeAnswer = (Convert.ToDateTime(reader["DateTimeAnswer"]));

                    if (!(reader["DateTimeQuestion"] is DBNull))
                        dateTimeQuestion = (Convert.ToDateTime(reader["DateTimeQuestion"]));
                    feedBacks.Add(new FeedBack()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UserIdAsking = reader["UserIdAsking"].ToString(),
                        UserIdAnswering = reader["UserIdAnswering"].ToString(),
                        DateTimeQuestion = dateTimeQuestion,
                        Question = reader["Question"].ToString(),
                        DateTimeAnswer = dateTimeAnswer,
                        Answer = reader["Answer"].ToString(),
                        IsAnswered = Convert.ToBoolean(reader["IsAnswered"])
                    });
                }
            }

            return feedBacks;
        }

        public void Update(FeedBack item)
        {
            var query = "UPDATE FeedBacks SET UserIdAsking = @UserIdAsking, UserIdAnswering = @UserIdAnswering, DateTimeQuestion = @DateTimeQuestion," +
                " Question = @Question, DateTimeAnswer = @DateTimeAnswer, Answer =@Answer, IsAnswered =@IsAnswered WHERE Id = @Id";
            var command = CreateCommand(query);

            command.Parameters.AddWithValue("@UserIdAsking", item.UserIdAsking);
            command.Parameters.AddWithValue("@UserIdAnswering", (object)item.UserIdAnswering ?? DBNull.Value);
            command.Parameters.AddWithValue("@DateTimeQuestion", (object)item.DateTimeQuestion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Question", item.Question);
            command.Parameters.AddWithValue("@DateTimeAnswer", (object)item.DateTimeAnswer ?? DBNull.Value);
            command.Parameters.AddWithValue("@Answer", (object)item.Answer ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsAnswered", item.IsAnswered);
            command.Parameters.AddWithValue("@Id", item.Id);

            command.ExecuteNonQuery();
        }
    }
}
