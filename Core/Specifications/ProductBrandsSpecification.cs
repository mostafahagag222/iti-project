using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductBrandsSpecification : BaseSpecification<ProductBrand>
    {
        public ProductBrandsSpecification(ProductSpecParams productParams) : base(x => 
            (string.IsNullOrEmpty(productParams.Search) ||
                x.Name.ToLower().Contains(productParams.Search) || 
                x.Origin.ToLower().Contains(productParams.Search)))
        {
            AddOrderBy(p => p.Name);

            var skip = productParams.PageSize * (productParams.PageIndex - 1);
            var take = productParams.PageSize;
            ApplyPaging(skip, take);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "rigin":
                        AddOrderBy(p => p.Origin);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductBrandsSpecification(int id) : base(x => x.Id == id)
        {

        }
    }
}
