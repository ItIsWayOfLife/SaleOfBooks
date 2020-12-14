using Core.DTO;
using System.Collections.Generic;



namespace Core.Interfaces
{
    public interface IOrderService
    {
        OrderDTO Create(string applicationUserId);
        IEnumerable<OrderDTO> GetOrders(string applicationUserId);
        IEnumerable<OrderBooksDTO> GetOrderBooks(string applicationUserId, int? orderId);
    }
}
