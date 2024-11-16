using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 9;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        private decimal? _priceFrom = 0;
        public decimal? PriceFrom 
        { 
            get => _priceFrom;
            set => _priceFrom = value > PriceTo? PriceTo: value;
        }

        private decimal? _priceTo = 100000;

        public decimal? PriceTo
        {
            get => _priceTo;
            set => _priceTo = value < PriceFrom ? PriceFrom : value;
        }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Color { get; set; }
        public string? Sort { get; set; }

        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
