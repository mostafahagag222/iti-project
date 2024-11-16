using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ReviewSpecification : BaseSpecification<Review>
    {
        private int TotalReviews { get; set; }
        private int WeightedAverage { get; set; }
        private int[] Stars { get; set; }
        private int Rating { get; set; }

        public ReviewSpecification(int productId = -1) : base()
        {
            Stars = new int[5];
            // Default Ordering
            AddOrderByDescending(r => r.Date);

            if (productId != -1)
            {
                AddCriteria(r => r.ProductId == productId);
            }

            AddInclude(r => r.Product);
            AddInclude(r => r.User);
        }

        public int[] getStars()
        {
            return Stars;
        }

        public void GetTotalReviews(IReadOnlyList<Review> reviews)
        {
            foreach (var item in reviews)
            {
                switch (item.Stars)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        Stars[item.Stars - 1] += 1;
                        break;
                }

                TotalReviews += 1;
            }
        }

        public int CalculateRating()
        {
            var counter = 1;
            foreach (var item in Stars)
            {
                WeightedAverage += counter * item;
                counter++;
            }
            if (TotalReviews != 0)
            {
                Rating = WeightedAverage / TotalReviews;
            }

            return Rating;
        }

    }
}
