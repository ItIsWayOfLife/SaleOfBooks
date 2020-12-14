using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;

namespace Core.Converters
{
    public  class ConverterFeedBack : IConverter<FeedBack, FeedBackDTO>
    {
        public FeedBack ConvertDTOByModel(FeedBackDTO modelDTO)
        {
            FeedBack feedBack = new FeedBack()
            {
                Id = modelDTO.Id,
                Answer = modelDTO.Answer,
                DateTimeAnswer = modelDTO.DateTimeAnswer,
                DateTimeQuestion = modelDTO.DateTimeQuestion,
                IsAnswered = modelDTO.IsAnswered,
                Question = modelDTO.Question,
                UserIdAnswering = modelDTO.UserIdAnswering,
                UserIdAsking = modelDTO.UserIdAsking
            };

            return feedBack;
        }

        public IEnumerable<FeedBack> ConvertDTOsByModels(IEnumerable<FeedBackDTO> feedBackDTOs)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FeedBackDTO, FeedBack>()).CreateMapper();
            var feedBacks = mapper.Map<IEnumerable<FeedBackDTO>, List<FeedBack>>(feedBackDTOs);

            return feedBacks;
        }

        public IEnumerable<FeedBackDTO> ConvertModelsByDTOs(IEnumerable<FeedBack> models)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FeedBack, FeedBackDTO>()).CreateMapper();
            var fedBacks = mapper.Map<IEnumerable<FeedBack>, List<FeedBackDTO>>(models);

            return fedBacks;
        }

        public FeedBackDTO ConvertModelByDTO(FeedBack model)
        {
            FeedBackDTO feedBackDTO = new FeedBackDTO()
            {
                Id = model.Id,
                UserIdAsking = model.UserIdAsking,
                UserIdAnswering = model.UserIdAnswering,
                Question = model.Question,
                IsAnswered = model.IsAnswered,
                DateTimeQuestion = model.DateTimeQuestion,
                DateTimeAnswer = model.DateTimeAnswer,
                Answer = model.Answer
            };

            return feedBackDTO;
        }
    }
}
