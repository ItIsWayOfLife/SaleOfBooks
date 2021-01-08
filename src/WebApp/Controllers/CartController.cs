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
using Core.Exceptions;

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

            _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, LoggerConstants.ERROR_USER_NOT_AUTHENTICATED, GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Delete(int cartBookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    _cartService.DeleteCartBook(cartBookId, GetCurrentUserId());
                }
                catch(ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE + $"/{cartBookId}", LoggerConstants.TYPE_POST, $"delete cartBookId: {cartBookId} error: {ex.Message}", GetCurrentUserId());

                    return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
                }

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE +$"/{cartBookId}", LoggerConstants.TYPE_POST, $"delete cartBookId: {cartBookId} successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_DELETE + $"/{cartBookId}", LoggerConstants.TYPE_POST, LoggerConstants.ERROR_USER_NOT_AUTHENTICATED, GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Add(int bookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    _cartService.AddBookToCart(bookId, GetCurrentUserId());
                }
                catch (ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD + $"{bookId}", LoggerConstants.TYPE_GET, $"add book id: {bookId} to cart error: {ex.Message}", GetCurrentUserId());

                    return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
                }

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_ADD +$"{bookId}", LoggerConstants.TYPE_GET, $"add book id: {bookId} to cart", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_ADD + $"{bookId}", LoggerConstants.TYPE_GET, LoggerConstants.ERROR_USER_NOT_AUTHENTICATED, GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult DeleteAll()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    _cartService.AllDeleteBooksToCart(GetCurrentUserId());
                }
                catch (ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + $"/deleteall", LoggerConstants.TYPE_POST, $"delete all books in cart error: {ex.Message}", GetCurrentUserId());

                    return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
                }

                _loggerService.LogInformation(CONTROLLER_NAME + $"/deleteall", LoggerConstants.TYPE_POST, "delete all books in cart successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogWarning(CONTROLLER_NAME + $"/deleteall", LoggerConstants.TYPE_POST, LoggerConstants.ERROR_USER_NOT_AUTHENTICATED, GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Update(int bookCartId, int count)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    _cartService.UpdateCountBookInCart(GetCurrentUserId(), bookCartId, count);
                }
                catch (ValidationException ex)
                {
                    _loggerService.LogWarning(CONTROLLER_NAME + $"/{bookCartId}&{count}", LoggerConstants.TYPE_POST, $"update book id: {bookCartId} on count: {count} error: {ex.Message}", GetCurrentUserId());

                    return RedirectToAction("Error", "Home", new { requestId = "400", errorInfo = ex.Message });
                }

                _loggerService.LogInformation(CONTROLLER_NAME + $"/{bookCartId}&{count}", LoggerConstants.TYPE_POST, $"update book id: {bookCartId} on count: {count} successful", GetCurrentUserId());

                return RedirectToAction("Index");
            }

            _loggerService.LogWarning(CONTROLLER_NAME + $"/{bookCartId}&{count}", LoggerConstants.TYPE_POST, LoggerConstants.ERROR_USER_NOT_AUTHENTICATED, GetCurrentUserId());

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
