using Core.Constants;
using Core.DTO;
using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Models.FeedBack;

namespace WebApp.Controllers
{
    [Authorize]
    public class FeedBackController : Controller
    {
        private readonly IFeedBackService _feedBackService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedBackController(IFeedBackService feedBackService, UserManager<ApplicationUser> userManager)
        {
            _feedBackService = feedBackService;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin, helper")]
        public IActionResult Index(string active)
        {
            var listFeedBacks = _feedBackService.GetFeedBacks();

            List<FeedBackViewModel> feedBackViewModels = new List<FeedBackViewModel>();

            foreach (var fb in listFeedBacks)
            {
                var userAsking = _userManager.Users.FirstOrDefault(p => p.Id == fb.UserIdAsking);
                var userAnswering = _userManager.Users.FirstOrDefault(p => p.Id == fb.UserIdAnswering);

                if (userAnswering != null)
                {
                    feedBackViewModels.Add(new FeedBackViewModel()
                    {
                        Id = fb.Id,
                        Answer = fb?.Answer,
                        DateTimeAnswer = fb?.DateTimeAnswer,
                        DateTimeQuestion = fb?.DateTimeQuestion,
                        IsAnswered = fb.IsAnswered,
                        Question = fb?.Question,
                        UserIdAnswering = fb?.UserIdAnswering,
                        UserIdAsking = fb?.UserIdAsking,
                        UserAnsweringLFP = userAnswering.LFP(),
                        UserAnsweringPath = PathConstants.PAPH_USERS + userAnswering.Path,
                        UserAskingLFP = userAsking.LFP(),
                        UserAskingPath = PathConstants.PAPH_USERS + userAsking.Path
                    });
                }
                else
                {
                    feedBackViewModels.Add(new FeedBackViewModel()
                    {
                        Id = fb.Id,
                        DateTimeQuestion = fb?.DateTimeQuestion,
                        IsAnswered = fb.IsAnswered,
                        Question = fb?.Question,
                        UserIdAsking = fb?.UserIdAsking,
                        UserAskingLFP = userAsking.LFP(),
                        UserAskingPath = PathConstants.PAPH_USERS + userAsking.Path,
                    });
                }
            }

            List<string> listActive = new List<string>()
            {
             "All",
             "Active",
             "Inactive"
            };

            if (listActive[1] == active)
            {
                feedBackViewModels = feedBackViewModels.Where(p => p.IsAnswered == false).ToList();
            }
            else if (listActive[2] == active)
            {
                feedBackViewModels = feedBackViewModels.Where(p => p.IsAnswered == true).ToList();
            }

            feedBackViewModels = feedBackViewModels.OrderByDescending(p => p.DateTimeQuestion).ToList();

            return View(new ListFeedBackViewModel() { FeedBackViews = feedBackViewModels, Active = active, ListActive = new SelectList(listActive) });
        }


        public IActionResult MyFeedBack()
        {
            string currentUserId = GetCurrentUserId();

            var listMyFeedBack = _feedBackService.GetMyFeedBack(currentUserId).OrderByDescending(p => p.DateTimeQuestion);

            List<FeedBackViewModel> feedBackViewModels = new List<FeedBackViewModel>();

            foreach (var fb in listMyFeedBack)
            {
                var userAsking = _userManager.Users.FirstOrDefault(p => p.Id == fb.UserIdAsking);
                var userAnswering = _userManager.Users.FirstOrDefault(p => p.Id == fb.UserIdAnswering);

                if (userAnswering != null)
                {
                    feedBackViewModels.Add(new FeedBackViewModel()
                    {
                        Id = fb.Id,
                        Answer = fb?.Answer,
                        DateTimeAnswer = fb?.DateTimeAnswer,
                        DateTimeQuestion = fb?.DateTimeQuestion,
                        IsAnswered = fb.IsAnswered,
                        Question = fb?.Question,
                        UserIdAnswering = fb?.UserIdAnswering,
                        UserIdAsking = fb?.UserIdAsking,
                        UserAnsweringLFP = userAnswering.LFP(),
                        UserAnsweringPath = PathConstants.PAPH_USERS + userAnswering.Path,
                        UserAskingLFP = userAsking.LFP(),
                        UserAskingPath = PathConstants.PAPH_USERS + userAsking.Path
                    });
                }
                else
                {
                    feedBackViewModels.Add(new FeedBackViewModel()
                    {
                        Id = fb.Id,
                        DateTimeQuestion = fb?.DateTimeQuestion,
                        IsAnswered = fb.IsAnswered,
                        Question = fb?.Question,
                        UserIdAsking = fb?.UserIdAsking,
                        UserAskingLFP = userAsking.LFP(),
                        UserAskingPath = PathConstants.PAPH_USERS + userAsking.Path,
                    });
                }
            }

            return View(new ListFeedBackViewModel() { FeedBackViews = feedBackViewModels });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string question)
        {
            string currentUserId = GetCurrentUserId();

            FeedBackDTO feedBack = new FeedBackDTO()
            {
                DateTimeQuestion = DateTime.Now,
                IsAnswered = false,
                Question = question,
                UserIdAsking = currentUserId
            };

            _feedBackService.AddQuestion(feedBack);

            return Redirect("MyFeedBack");
        }

        [HttpGet]
        [Authorize(Roles = "admin, helper")]
        public IActionResult AddAnswer(int id)
        {
            var fb = _feedBackService.GetFeedBack(id);

            FeedBackViewModel feedBackView = null;

            if (fb == null)
                return RedirectToAction("Index");

            var userAsking = _userManager.Users.FirstOrDefault(p => p.Id == fb.UserIdAsking);
            var userAnswering = _userManager.Users.FirstOrDefault(p => p.Id == fb.UserIdAnswering);

            if (userAnswering != null)
            {

                feedBackView = new FeedBackViewModel()
                {
                    Id = fb.Id,
                    Answer = fb?.Answer,
                    Question = fb?.Question,
                };
            }
            else
            {
                feedBackView = new FeedBackViewModel()
                {
                    Id = fb.Id,
                    Question = fb?.Question,
                };
            }

            return View(feedBackView);
        }

        [HttpPost]
        [Authorize(Roles = "admin, helper")]
        public IActionResult AddAnswer(FeedBackViewModel model)
        {
            var fb = _feedBackService.GetFeedBack(model.Id);

            string currentUserId = GetCurrentUserId();

            fb.Answer = model.Answer;
            fb.DateTimeAnswer = DateTime.Now;
            fb.IsAnswered = true;
            fb.UserIdAnswering = currentUserId;

            _feedBackService.AddAnswer(fb);

            return RedirectToAction("Index");
        }

        private string GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            else
            {
                return null;
            }
        }
    }
}
