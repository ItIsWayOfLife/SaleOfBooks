using Core.Constants;
using Core.DTO;
using Core.Exceptions;
using Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Models.Order;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ILoggerService _loggerService;

        private const string CONTROLLER_NAME = "order";

        public OrderController(IOrderService orderService,
            ICartService cartService,
            ILoggerService loggerService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Index(SortState sortOrder = SortState.DateOrderDesc)
        {
            if (User.Identity.IsAuthenticated)
            {           
                    IEnumerable<OrderDTO> orderDTOs = _orderService.GetOrders(GetCurrentUserId());
                    var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderDTO, BookViewModel>()).CreateMapper();
                    var orders = mapper.Map<IEnumerable<OrderDTO>, List<BookViewModel>>(orderDTOs);

                    ViewData["IdSort"] = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
                    ViewData["DateSort"] = sortOrder == SortState.DateOrderAsc ? SortState.DateOrderDesc : SortState.DateOrderAsc;
                    ViewData["FullPriceSort"] = sortOrder == SortState.FullPriceAsc ? SortState.FullPriceDesc : SortState.FullPriceAsc;
                    ViewData["CountDishSort"] = sortOrder == SortState.CountBookAsc ? SortState.CountBookDesc : SortState.CountBookAsc;

                    orders = sortOrder switch
                    {
                        SortState.IdDesc => orders.OrderByDescending(s => s.Id).ToList(),
                        SortState.DateOrderAsc => orders.OrderBy(s => s.DateOrder).ToList(),
                        SortState.DateOrderDesc => orders.OrderByDescending(s => s.DateOrder).ToList(),
                        SortState.FullPriceAsc => orders.OrderBy(s => s.FullPrice).ToList(),
                        SortState.FullPriceDesc => orders.OrderByDescending(s => s.FullPrice).ToList(),
                        SortState.CountBookAsc => orders.OrderBy(s => s.CountBook).ToList(),
                        SortState.CountBookDesc => orders.OrderByDescending(s => s.CountBook).ToList(),
                        _ => orders.OrderBy(s => s.Id).ToList(),
                    };

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "index", GetCurrentUserId());

                return View(orders);               
            }

            _loggerService.LogWarning(CONTROLLER_NAME + LoggerConstants.ACTION_INDEX, LoggerConstants.TYPE_GET, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = GetCurrentUserId();

                _orderService.Create(currentUserId);
                _cartService.AllDeleteBooksToCart(currentUserId);

                _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_CREATE, LoggerConstants.TYPE_GET, "index", GetCurrentUserId());

                return RedirectToAction($"Index");
            }

            _loggerService.LogInformation(CONTROLLER_NAME + LoggerConstants.ACTION_CREATE, LoggerConstants.TYPE_GET, "not authenticated", GetCurrentUserId());

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult GetOrderBooks(int orderId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = GetCurrentUserId();

                IEnumerable<OrderBooksDTO> orderBooksDTOs = _orderService.GetOrderBooks(currentUserId, orderId);
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrderBooksDTO, OrderBooksViewModel>()).CreateMapper();
                var orderDishes = mapper.Map<IEnumerable<OrderBooksDTO>, List<OrderBooksViewModel>>(orderBooksDTOs);

                foreach (var oD in orderDishes)
                {
                    oD.Path = PathConstants.PAPH_BOOKS + oD.Path;
                }

                ViewData["FullPrice"] = _orderService.GetOrders(currentUserId).Where(p => p.Id == orderId).FirstOrDefault().FullPrice;

                _loggerService.LogInformation(CONTROLLER_NAME + $"/getorderbooks/{orderId}", LoggerConstants.TYPE_GET, $"get order books", GetCurrentUserId());

                return View(orderDishes);
            }

            _loggerService.LogInformation(CONTROLLER_NAME + $"/getorderbooks/{orderId}", LoggerConstants.TYPE_GET, $"not authenticated", GetCurrentUserId());

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
