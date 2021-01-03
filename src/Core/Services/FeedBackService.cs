using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class FeedBackService : IFeedBackService
    {
        private IUnitOfWork Database { get; set; }

        private readonly IConverter<FeedBack, FeedBackDTO> _converterFeedBack;

        public FeedBackService(IUnitOfWork uow,
           IConverter<FeedBack, FeedBackDTO> converterFeedBack)
        {
            Database = uow;
            _converterFeedBack = converterFeedBack;
        }
      
        public FeedBackDTO GetFeedBack(int? id)
        {
            if (id == null)
                throw new ValidationException("Id feedBack not found", "");

            var feedBack = Database.FeedBack.Get(id.Value);

            if (feedBack == null)
                throw new ValidationException($"FeedBack with id {id} not found", "");

            return _converterFeedBack.ConvertModelByDTO(feedBack);
        }

        public IEnumerable<FeedBackDTO> GetFeedBacks()
        {
            var feedBackDTOs = Database.FeedBack.GetAll().OrderByDescending(p=>p.DateTimeAnswer);

            return _converterFeedBack.ConvertModelsByDTOs(feedBackDTOs);
        }

        public IEnumerable<FeedBackDTO> GetMyFeedBack(string userId)
        {
            return GetFeedBacks().ToList().Where(p => p.UserIdAsking == userId).OrderBy(p=>p.DateTimeAnswer);
        }

        public void AddQuestion(FeedBackDTO feedBackDTO)
        {
            Database.FeedBack.Create(_converterFeedBack.ConvertDTOByModel(feedBackDTO));
            Database.Save();
        }

        public void AddAnswer(FeedBackDTO feedBackDTO)
        {
            FeedBack feedBack = Database.FeedBack.Get(feedBackDTO.Id);

            if (feedBack == null)
                throw new ValidationException($"Feedback with id {feedBackDTO.Id} not found", "");

            feedBack.Answer = feedBackDTO.Answer;
            feedBack.DateTimeAnswer = feedBackDTO.DateTimeAnswer;
            feedBack.UserIdAnswering = feedBackDTO.UserIdAnswering;
            feedBack.IsAnswered = feedBackDTO.IsAnswered;

            Database.FeedBack.Update(feedBack);
            Database.Save();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new ValidationException("Id feedBack not found", "");

            var feedBack = Database.FeedBack.Get(id.Value);

            if (feedBack == null)
                throw new ValidationException($"FeedBack with id {id} not found", "");

            Database.FeedBack.Delete(id.Value);
            Database.Save();
        }

        public void Edit(FeedBackDTO feedBackDTO)
        {
            Database.FeedBack.Update(_converterFeedBack.ConvertDTOByModel(feedBackDTO));
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public int GetCountActive()
        {
            return Database.FeedBack.GetAll().ToList().Where(p => p.IsAnswered == false).Count();
        }
    }
}
