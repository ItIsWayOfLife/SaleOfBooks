﻿using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using Core.Exceptions;
using System.Linq;

namespace Core.Services
{
    public class OrderService:IOrderService
    {
        private IUnitOfWork Database { get; set; }

        public OrderService(IUnitOfWork uow)
        {
            Database = uow;
        }
    
        public OrderDTO Create(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", string.Empty);

            var cart = Database.Cart.Find(p => p.ApplicationUserId == applicationUserId).FirstOrDefault();

            if (cart == null)
                throw new ValidationException("Cart not found", string.Empty);

            var cartBooks = Database.CartBooks.Find(p => p.CartId == cart.Id).ToList();

            if (cartBooks == null || cartBooks.Count() == 0)
                throw new ValidationException("Cart is empty", string.Empty);

            // create order
            Order order = new Order() { ApplicationUserId = applicationUserId, DateOrder = DateTime.Now };

            Database.Order.Create(order);
            Database.Save();

            foreach (var cartBook in cartBooks)
            {
                Database.OrderBooks.Create(new OrderBooks()
                {
                    Count = cartBook.Count,
                    BookId = cartBook.BookId,
                    OrderId = order.Id
                });
            }

            Database.Save();

            OrderDTO orderDTO = new OrderDTO()
            {
                CountBook = cartBooks.Sum(p => p.Count),
                DateOrder = order.DateOrder,
                FullPrice = FullPriceOrder(Database.OrderBooks.Find(p => p.OrderId == order.Id)),
                Id = order.Id
            };

            return orderDTO;
        }

        public IEnumerable<OrderDTO> GetOrders(string applicationUserId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", string.Empty);

            var orders = Database.Order.Find(p => p.ApplicationUserId == applicationUserId);

            var ordersDTOs = new List<OrderDTO>();

            foreach (var order in orders)
            {
                ordersDTOs.Add(new OrderDTO()
                {
                    CountBook = Database.OrderBooks.Find(p => p.OrderId == order.Id).Sum(p => p.Count),
                    Id = order.Id,
                    DateOrder = order.DateOrder,
                    FullPrice = FullPriceOrder(Database.OrderBooks.Find(p => p.OrderId == order.Id))
                });
            }

            return ordersDTOs;
        }

        public IEnumerable<OrderBooksDTO> GetOrderBooks(string applicationUserId, int? orderId)
        {
            if (applicationUserId == null)
                throw new ValidationException("User id not set", string.Empty);

            if (orderId == null)
                throw new ValidationException("Order not selected", string.Empty);

            var orderBooks = Database.OrderBooks.Find(p => p.OrderId == orderId).Where(p => p.Order.ApplicationUserId == applicationUserId);

            var orderBooksDTOs = new List<OrderBooksDTO>();

            foreach (var orderBook in orderBooks)
            {
                orderBooksDTOs.Add(new OrderBooksDTO()
                {
                    Count = orderBook.Count,

                    BookId = orderBook.BookId,
                    Id = orderBook.Id,
                    OrderId = orderBook.OrderId,
                    Path = orderBook.Book.Path,
                    Price = orderBook.Book.Price,
                    Code = orderBook.Book.Code,
                    Name = orderBook.Book.Name,
                });
            }

            return orderBooksDTOs;
        }

        private decimal FullPriceOrder(IEnumerable<OrderBooks> orderBooks)
        {
            decimal fullPrice = 0;

            foreach (var cartBook in orderBooks)
            {
                fullPrice += cartBook.Count * cartBook.Book.Price;
            }

            return fullPrice;
        }
    }
}
