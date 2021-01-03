using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebApp.Models.FeedBack
{
    public class ListFeedBackViewModel
    {
        public List<FeedBackViewModel> FeedBackViews { get; set; }
        public string Active { get; set; }
        public SelectList ListActive { get; set; }
    }
}
