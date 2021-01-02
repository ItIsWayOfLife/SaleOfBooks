using Core.Constants;
using Core.DTO;
using Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using WebApp.Models.Cart;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILoggerService _loggerService;

        private const string CONTROLLER_NAME = "cart";

        public CartController(ICartService cartService,
            ILoggerService loggerService)
        {
            _cartService = cartService;
            _loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = GetCurrentUserId();

                var cartDTO = _cartService.GetCart(currentUserId);

                var cartBooksDTO = _cartService.GetCartBooks(currentUserId);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CartBooksDTO, CartDishesViewModel>()).CreateMapper();
                var cartDishes = mapper.Map<IEnumerable<CartBooksDTO>, List<CartDishesViewModel>>(cartBooksDTO);

                foreach (var cD in cartDishes)
                {
                    cD.Path = PathConstants.PAPH_BOOKS + cD.Path;
                }

                ViewData["FullPrice"] = _cartService.FullPriceCart(currentUserId);

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "index", GetCurrentUserId());

                return View(cartDishes);
            }

            _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Delete(int cartBookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.DeleteCartBook(cartBookId, GetCurrentUserId());

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE +$"/{cartBookId}", LoggerConstants.TYPE_POST, "delete successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE + $"/{cartBookId}", LoggerConstants.TYPE_POST, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Add(int bookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.AddBookToCart(bookId, GetCurrentUserId());

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD +$"{bookId}", LoggerConstants.TYPE_GET, "add", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD + $"{bookId}", LoggerConstants.TYPE_GET, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult DeleteAll()
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.AllDeleteBooksToCart(GetCurrentUserId());

                _loggerService.LogInformation(CONTROLLER_NAME + $"/deleteall", LoggerConstants.TYPE_POST, "delete successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogInformation(CONTROLLER_NAME + $"/deleteall", LoggerConstants.TYPE_POST, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Update(int bookCartId, int count)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.UpdateCountBookInCart(GetCurrentUserId(), bookCartId, count);

              _loggerService.LogInformation(CONTROLLER_NAME + $"/{bookCartId}&{count}", LoggerConstants.TYPE_POST, "update successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogInformation(CONTROLLER_NAME + $"/{bookCartId}&{count}", LoggerConstants.TYPE_POST, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
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
