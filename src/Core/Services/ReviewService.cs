using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            throw new NotImplementedException();
        }

        public IEnumerable<ReviewDTO> GetReviews()
        {
            var reviewDTOs = Database.Review.GetAll();
            return _convertReview.ConvertModelsByDTOs(reviewDTOs);
        }

        public void Add(ReviewDTO reviewDTO)
        {
            throw new NotImplementedException();
        }

        public void Edit(ReviewDTO reviewDTO)
        {
            throw new NotImplementedException();
        }

        public void Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
