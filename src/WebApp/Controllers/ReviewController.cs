using Core.Constants;
using Core.DTO;
using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Models.Review;

namespace WebApp.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ILikeService _likeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IReviewService reviewService,
            ILikeService likeService,
             UserManager<ApplicationUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
            _likeService = likeService;
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

            return View(new ListReviewViewModel() { ReviewViewModels = reviewViewModels });
        }

        [HttpPost]
        public IActionResult AddLike(int idReview)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            string currentUserId = GetCurrentUserId();

            bool like = _likeService.CheckLike(currentUserId, idReview);

            if (like == true)
            {
                _likeService.Delete(currentUserId, idReview);
            }
            else
            {
                _likeService.Add(currentUserId, idReview);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddEditReviewViewModel model)
        {
            string currentUserId = GetCurrentUserId();

            _reviewService.Add(new ReviewDTO()
            {
                ApplicationUserId = currentUserId,
                Content = model.Content
            });

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var reviewDTO = _reviewService.GetReview(id);

            return View(new AddEditReviewViewModel() {  Id = reviewDTO.Id, Content = reviewDTO.Content});
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(AddEditReviewViewModel model)
        {
            _reviewService.Edit(new ReviewDTO() {  Id = model.Id, Content = model.Content});

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _reviewService.Delete(id);

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
