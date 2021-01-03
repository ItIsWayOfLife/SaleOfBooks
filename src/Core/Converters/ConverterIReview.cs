using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Core.Converters
{
    public class ConverterIReview : IConverter<Review, ReviewDTO>
    {
        public Review ConvertDTOByModel(ReviewDTO modelDTO)
        {
            Review review = new Review()
            {
                Id = modelDTO.Id,
                ApplicationUserId = modelDTO.ApplicationUserId,
                Content = modelDTO.Content,
                DateTime = modelDTO.DateTime
            };

            return review;
        }

        public IEnumerable<Review> ConvertDTOsByModels(IEnumerable<ReviewDTO> modelDTOs)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ReviewDTO, Review>()).CreateMapper();
            var reviews = mapper.Map<IEnumerable<ReviewDTO>, List<Review>>(modelDTOs);

            return reviews;
        }

        public ReviewDTO ConvertModelByDTO(Review model)
        {
            ReviewDTO reviewDTO = new ReviewDTO()
            {
                Id = model.Id,
                ApplicationUserId = model.ApplicationUserId,
                Content = model.Content,
                DateTime = model.DateTime
            };

            return reviewDTO;
        }

        public IEnumerable<ReviewDTO> ConvertModelsByDTOs(IEnumerable<Review> models)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Review, ReviewDTO>()).CreateMapper();
            var reviewDTOs = mapper.Map<IEnumerable<Review>, List<ReviewDTO>>(models);

            return reviewDTOs;
        }
    }
}
