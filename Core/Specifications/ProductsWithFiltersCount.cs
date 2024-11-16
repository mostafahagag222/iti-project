using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsWithFiltersCount : BaseSpecification<Product>
    {
        public ProductsWithFiltersCount(ProductSpecParams productParams) :
            base(x =>
                (string.IsNullOrEmpty(productParams.Search) || (x.Name.ToLower().Contains(productParams.Search) || x.ProductBrand.Name.ToLower().Contains(productParams.Search) || x.ProductType.Name.ToLower().Contains(productParams.Search))) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) &&
                (!productParams.PriceFrom.HasValue || x.Price >= productParams.PriceFrom) &&
                (!productParams.PriceTo.HasValue || x.Price <= productParams.PriceTo) &&
                (string.IsNullOrEmpty(productParams.Color) || x.Color.ToLower() == productParams.Color.ToLower())
            )
        {

        }
    }
}
