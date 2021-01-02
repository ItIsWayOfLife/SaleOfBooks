using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Core.Services
{
    public class ReviewService: IReviewService
    {
        private IUnitOfWork Database { get; set; }

        private readonly IConverter<Review, ReviewDTO> _convertReview;

        public ReviewService(IUnitOfWork uow,
            IConverter<Review, ReviewDTO> convertReview)
        {
            Database = uow;
            _convertReview = convertReview;
        }

        public ReviewDTO GetReview(int? id)
        {
            if (id == null)
                throw new ValidationException("Review id not set", "");

            var review = Database.Review.Get(id.Value);

            if (review == null)
                throw new ValidationException("Review not fuund", "");

            var reviewDTO = _convertReview.ConvertModelByDTO(review);

            reviewDTO.CountLikes = GetCountLike(reviewDTO.Id);

            return reviewDTO;
        }

        public IEnumerable<ReviewDTO> GetReviews()
        {
            var reviews = Database.Review.GetAll();

            var reviewDTOs = _convertReview.ConvertModelsByDTOs(reviews);

            foreach (var revDTO in reviewDTOs)
            {
                revDTO.CountLikes = GetCountLike(revDTO.Id);
            }

            return reviewDTOs;
        }

        public void Add(ReviewDTO reviewDTO)
        {
            Database.Review.Create(
            new Review()
            {
                ApplicationUserId = reviewDTO.ApplicationUserId,
                Content = reviewDTO.Content,
                DateTime = DateTime.Now
            });

            Database.Save();
        }

        public void Edit(ReviewDTO reviewDTO)
        {
            Review review = Database.Review.Get(reviewDTO.Id);

            if (review == null)
                throw new ValidationException("Review not fuund", "");

            review.Content = reviewDTO.Content;

            Database.Review.Update(review);
            Database.Save();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new ValidationException("Review id not set", "");

            var review = Database.Review.Get(id.Value);

            if (review == null)
                throw new ValidationException("Review not fuund", "");

            Database.Review.Delete(review.Id);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private int GetCountLike(int reviewId)
        {
            IEnumerable<Like> likesList = Database.Like.GetAll();

            int count = 0;

            foreach (var like in likesList)
            {
                if (like.ReviewId == reviewId)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
