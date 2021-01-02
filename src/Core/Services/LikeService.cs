using Core.Entities;
using Core.Interfaces;
using System.Linq;

namespace Core.Services
{
    public class LikeService : ILikeService
    {
        private IUnitOfWork Database { get; set; }

        public LikeService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Add(string userId, int reviewId)
        {
            if (!CheckLike(userId, reviewId))
            {
                Database.Like.Create(new Like() { ReviewId = reviewId, UserId = userId });
                Database.Save();
            }
        }

        public void Delete(string userId, int reviewId)
        {
            if (CheckLike(userId, reviewId))
            {
                var like = Database.Like.GetAll().ToList().Where(p => p.UserId == userId).Where(p => p.ReviewId == reviewId).FirstOrDefault();

                Database.Like.Delete(like.Id);
                Database.Save();
            }
        }

        public bool CheckLike(string userId, int reviewId)
        {
            var listLike = Database.Like.GetAll().ToList().Where(p => p.UserId == userId).Where(p => p.ReviewId == reviewId).ToList();

            if (listLike.Count > 0)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
