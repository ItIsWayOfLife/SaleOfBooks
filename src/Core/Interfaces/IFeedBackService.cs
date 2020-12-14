using Core.DTO;
using System.Collections.Generic;

namespace Core.Interfaces
{
   public interface IFeedBackService
    {
        FeedBackDTO GetFeedBack(int? id);
        IEnumerable<FeedBackDTO> GetFeedBacks();
        void Add(FeedBackDTO feedBackDTO);
        void Edit(FeedBackDTO feedBackDTO);
        void Delete(int? id);
        void Dispose();
    }
}
