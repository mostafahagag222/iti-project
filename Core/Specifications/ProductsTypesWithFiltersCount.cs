using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Specifications
{
    public class ProductsTypesWithFiltersCount : BaseSpecification<ProductType>
    {
        public ProductsTypesWithFiltersCount(ProductSpecParams productParams) :
           base(x =>
               (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))
               )
        {

        }
    }

}
