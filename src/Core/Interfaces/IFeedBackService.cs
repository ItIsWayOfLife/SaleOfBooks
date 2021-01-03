using Core.DTO;
using System.Collections.Generic;

namespace Core.Interfaces
{
   public interface IFeedBackService
    {
        FeedBackDTO GetFeedBack(int? id);
        IEnumerable<FeedBackDTO> GetFeedBacks();
        IEnumerable<FeedBackDTO> GetMyFeedBack(string userId);
        void AddQuestion(FeedBackDTO feedBackDTO);
        void AddAnswer(FeedBackDTO feedBackDTO);
        void Delete(int? id);
        void Edit(FeedBackDTO feedBackDTO);
        int GetCountActive();
        void Dispose();
    }
}
