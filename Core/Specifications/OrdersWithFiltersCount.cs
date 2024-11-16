using Core.Entities;
using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrdersWithFiltersCount : BaseSpecification<Order>
    {
        public OrdersWithFiltersCount(OrderSpecParams orderParams) : base(x =>
            (!orderParams.Search.HasValue || (int)x.Status == orderParams.Search) &&
            (!orderParams.SubTotalFrom.HasValue || x.Subtotal >= orderParams.SubTotalFrom) &&
            (!orderParams.SubTotalTo.HasValue || x.Subtotal <= orderParams.SubTotalTo)
        )
        {
            Debug.Print(OrderStatus.Pending.ToString());
        }
    }
}
