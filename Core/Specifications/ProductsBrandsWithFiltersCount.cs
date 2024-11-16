using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsBrandsWithFiltersCount : BaseSpecification<ProductBrand>
    {
        public ProductsBrandsWithFiltersCount(ProductSpecParams productBrandsParams) :
           base(x =>
               (string.IsNullOrEmpty(productBrandsParams.Search) || x.Name.ToLower().Contains(productBrandsParams.Search))
               )
        {

        }
    }

}
