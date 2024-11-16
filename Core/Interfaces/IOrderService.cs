using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, int basketId, Entities.OrderAggregate.Address shippingToAddress);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);

        Task<Order> GetOrderById(int id);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
        
        Task CancelOrderAsync(int id);

        Task UpdateOrder(int id, Order order);

        Task<IReadOnlyList<Order>> GetAllOrdersAsync();

        Task<IReadOnlyList<Order>> GetAllOrdersAsync(ISpecification<Order> spec);

        Task<int> CountAsync(ISpecification<Order> spec);

        Task<Order> confirmOrder(int id, Order order);
    }
}
