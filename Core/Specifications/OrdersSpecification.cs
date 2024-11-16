using Core.Entities;
using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrdersSpecification : BaseSpecification<Order>
    {
        public OrdersSpecification()
        {
            
        }
        public OrdersSpecification(OrderSpecParams orderParams) : base(x =>
            (!orderParams.Search.HasValue|| (int)x.Status == orderParams.Search) &&
            (!orderParams.SubTotalFrom.HasValue || x.Subtotal >= orderParams.SubTotalFrom) &&
            (!orderParams.SubTotalTo.HasValue || x.Subtotal <= orderParams.SubTotalTo)
        )
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.OrderItems);
            AddOrderBy(x => x.Subtotal);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);

            if (!string.IsNullOrEmpty(orderParams.Sort))
            {
                switch (orderParams.Sort)
                {
                    case "subTotalAsc":
                        AddOrderBy(p => p.Subtotal);
                        break;
                    case "subTotalDesc":
                        AddOrderByDescending(p => p.Subtotal);
                        break;
                    default:
                        break;
                }
            }
        }
        public OrdersSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.DeliveryMethod);
        }
    }
}
