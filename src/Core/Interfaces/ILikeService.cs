using Core.Entities;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface ILikeService
    {
        Like GetLike(int? id);
        IEnumerable<Like> GetLikes();
        void Add(Like like);
        void Edit(Like like);
        void Delete(int? id);
        void Dispose();
    }
}
