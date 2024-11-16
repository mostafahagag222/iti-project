using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        private decimal? subTotalFrom = 0;
        public decimal? SubTotalFrom
        { 
            get => subTotalFrom;
            set => subTotalFrom = value > SubTotalTo ? SubTotalTo : value;
        }

        private decimal? subTotalTo = 100000;

        public decimal? SubTotalTo
        {
            get => subTotalTo;
            set => subTotalTo = value < SubTotalFrom ? SubTotalFrom : value;
        }

        public string Sort { get; set; }

        private int? _search;
        public int? Search
        {
            get => _search;
            set => _search = value;
        }
    }
}
