using Core.Constants;
using Core.DTO;
using Core.Exceptions;
using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models.Review;

namespace WebApp.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ILikeService _likeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggerService _loggerService;

        private const string CONTROLLER_NAME = "review";

        public ReviewController(IReviewService reviewService,
            ILikeService likeService,
             UserManager<ApplicationUser> userManager,
             ILoggerService loggerService)
        {
            _reviewService = reviewService;
            _userManager = userManager;
            _likeService = likeService;
            _loggerService = loggerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var listReviewDTO = _reviewService.GetReviews();

            List<ReviewViewModel> reviewViewModels = new List<ReviewViewModel>();

            string currentUserId = "";

            if (User.Identity.IsAuthenticated)
            {
                currentUserId = GetCurrentUserId();
            }

            foreach (var review in listReviewDTO)
            {
                var user = _userManager.Users.FirstOrDefault(p => p.Id == review.ApplicationUserId);

                bool like = _likeService.CheckLike(currentUserId, review.Id);

                reviewViewModels.Add(
                    new ReviewViewModel()
                    {
                        Id = review.Id,
                        ApplicationUserId = review.ApplicationUserId,
                        Content = review.Content,
                        CountLikes = review.CountLikes,
                        DateTime = review.DateTime,
                        Path = PathConstants.PAPH_USERS + user.Path,
                        LFP = user.LFP(),
                        Like = like
                    });

                reviewViewModels = reviewViewModels.OrderByDescending(p => p.DateTime).ToList();
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "index", GetCurrentUserId());

            return View(new ListReviewViewModel() { ReviewViewModels = reviewViewModels });
        }

        [HttpPost]
        public IActionResult AddLike(int idReview)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            string currentUserId = GetCurrentUserId();

            bool like = _likeService.CheckLike(currentUserId, idReview);

            try
            {
                if (like == true)
                {
                    _likeService.Delete(currentUserId, idReview);
                }
                else
                {
                    _likeService.Add(currentUserId, idReview);
                }
            }
            catch (ValidationException ex)
            {
                _loggerService.LogWarning(CONTROLLER_NAME + $"/addlike/{idReview}", LoggerConstants.TYPE_POST, $"add like review id: {idReview} error {ex.Message}", GetCurrentUserId());

                return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + $"/addlike/{idReview}", LoggerConstants.TYPE_POST, $"add like review id: {idReview} successful", GetCurrentUserId());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_GET, "add", GetCurrentUserId());

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddEditReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = GetCurrentUserId();

                _reviewService.Add(new ReviewDTO()
                {
                    ApplicationUserId = currentUserId,
                    Content = model.Content
                });

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD, LoggerConstants.TYPE_POST, $"add review id {model.Id} successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ReviewDTO reviewDTO = null;

            try
            {
                 reviewDTO = _reviewService.GetReview(id);
            }
            catch (ValidationException ex)
            {
                _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT + $"/{id}", LoggerConstants.TYPE_GET, $"edit review id: {id} error: {ex.Message}", GetCurrentUserId());

                return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT +$"/{id}", LoggerConstants.TYPE_GET, $"edit review id: {id}", GetCurrentUserId());

            return View(new AddEditReviewViewModel() { Id = reviewDTO.Id, Content = reviewDTO.Content });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(AddEditReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _reviewService.Edit(new ReviewDTO() { Id = model.Id, Content = model.Content });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);

                    return View(model);
                }

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_EDIT, LoggerConstants.TYPE_POST, $"edit review id: {model.Id} successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _reviewService.Delete(id);
            }
            catch (ValidationException ex)
            {
                _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE + $"/{id}", LoggerConstants.TYPE_POST, $"delete review id: {id} error: {ex.Message}", GetCurrentUserId());

                return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE +$"/{id}", LoggerConstants.TYPE_POST, $"delete review id: {id} successful", GetCurrentUserId());

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
