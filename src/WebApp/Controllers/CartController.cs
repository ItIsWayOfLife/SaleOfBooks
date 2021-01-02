using Core.Constants;
using Core.DTO;
using Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using WebApp.Models.Cart;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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

                return View(cartDishes);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Delete(int? cartBookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.DeleteCartBook(cartBookId, GetCurrentUserId());

                return RedirectToAction("Index");
            }

           return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Add(int? bookId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.AddBookToCart(bookId, GetCurrentUserId());

                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult DeleteAll()
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.AllDeleteBooksToCart(GetCurrentUserId());

                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Update(int? bookCartId, int count)
        {
            if (User.Identity.IsAuthenticated)
            {
                _cartService.UpdateCountBookInCart(GetCurrentUserId(), bookCartId, count);

                return RedirectToAction("Index");
            }

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
