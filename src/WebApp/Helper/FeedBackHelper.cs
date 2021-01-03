using Core.Interfaces;
using WebApp.Interfaces;

namespace WebApp.Helper
{
    public class FeedBackHelper : IFeedBackHelper
    {
        private readonly IFeedBackService _feedBackService;

        public FeedBackHelper(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public int GetCount()
        {
            return _feedBackService.GetCountActive();
        }
    }
}
