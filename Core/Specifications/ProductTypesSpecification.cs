using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductTypesSpecification : BaseSpecification<ProductType>
    {
        public ProductTypesSpecification(ProductSpecParams productParams) : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))
            )
        {
            AddOrderBy(p => p.Name);

            var skip = productParams.PageSize * (productParams.PageIndex - 1);
            var take = productParams.PageSize;
            ApplyPaging(skip, take);
        }

        public ProductTypesSpecification(int id) : base(x => x.Id == id)
        {

        }

    }
}
