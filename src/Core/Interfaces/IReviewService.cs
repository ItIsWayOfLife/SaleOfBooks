using Core.DTO;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IReviewService
    {
        ReviewDTO GetReview(int? id);
        IEnumerable<ReviewDTO> GetReviews();
        void Add(ReviewDTO reviewDTO);
        void Edit(ReviewDTO reviewDTO);
        void Delete(int? id);
        void Dispose();
    }
}
