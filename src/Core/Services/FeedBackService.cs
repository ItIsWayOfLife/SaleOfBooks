using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System.Collections.Generic;

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

        public void Add(FeedBackDTO feedBackDTO)
        {
            Database.FeedBack.Create(_converterFeedBack.ConvertDTOByModel(feedBackDTO));
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

        public void Dispose()
        {
            Database.Dispose();
        }

        public void Edit(FeedBackDTO feedBackDTO)
        {
            Database.FeedBack.Update(_converterFeedBack.ConvertDTOByModel(feedBackDTO));
            Database.Save();
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
            var feedBackDTOs = Database.FeedBack.GetAll();
            return _converterFeedBack.ConvertModelsByDTOs(feedBackDTOs);
        }
    }
}
