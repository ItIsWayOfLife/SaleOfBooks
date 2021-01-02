
namespace Core.Interfaces
{
    public interface ILikeService
    {
        void Delete(string userId, int reviewId);
        void Add(string userId, int reviewId);
        bool CheckLike(string userId, int reviewId);
        void Dispose();
    }
}
