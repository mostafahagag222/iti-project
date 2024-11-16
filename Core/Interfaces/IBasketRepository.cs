using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        public Task<Basket> GetBasketAsync(string userId);
        public Task<Basket> UpdateBasketAsync(Basket basket);
        public Task DeleteBasketAsync(int basketId);
        public Task<List<Product>> GetBasketProducts(int basketId);
    }
}
