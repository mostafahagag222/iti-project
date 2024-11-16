using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class WishListSpecification : BaseSpecification<WishList>
    {

        public WishListSpecification(string userId) : base()
        {
            // Default Ordering
            
             //AddCriteria(r => r.userId == userId);

            AddInclude(r => r.Products);
        }
    }
}
