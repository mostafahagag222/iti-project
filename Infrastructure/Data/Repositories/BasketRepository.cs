using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly StoreContext context;
        private readonly IGenericRepository<Product> productRepo;

        public BasketRepository(StoreContext context, IGenericRepository<Product> productRepo)
        {
            this.context = context;
            this.productRepo = productRepo;
        }
        public async Task<Basket> GetBasketAsync(string userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return null;
            var basket = await context.Baskets.SingleOrDefaultAsync(b => b.UserId == userId);
            if (basket == null)
            {
                var newBasket = new Basket()
                {
                    User = user,
                    UserId = userId
                };
                await context.Baskets.AddAsync(newBasket);
                await context.SaveChangesAsync();
                return newBasket;
            }
            else
            {
                var basketItems = context.BasketItems.Where(i => i.BasketId == basket.Id).ToList();
                basket.BasketItems = basketItems;
                return basket;
            }
        }
        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            var existingBasketItems = context.BasketItems.Where(i =>
            i.BasketId == basket.Id).ToList();
            if (basket.BasketItems.Count >= existingBasketItems.Count)
            {
                foreach (var item in basket.BasketItems)
                {
                    if (existingBasketItems.Any(i => i.ProductId == item.ProductId))
                    {
                        var foundItem = await context.BasketItems
                            .Where(bi => bi.BasketId == basket.Id).SingleOrDefaultAsync(
                            i => i.ProductId == item.ProductId);
                        foundItem.Quantity = item.Quantity;
                    }
                    else
                    {
                        context.BasketItems.Add(item);
                    }
                }
            }
            else
            {
                var deletedItem = existingBasketItems.Where(p => !basket.BasketItems
                .Any(p2 => p2.ProductId == p.ProductId)).SingleOrDefault();
                context.BasketItems.Remove(deletedItem);
            }

            // Update existing basket with payment information

            var existingBasket = context.Baskets.Find(basket.Id);
            existingBasket.ClientSecret = basket.ClientSecret;
            existingBasket.DeliveryMethodId = basket.DeliveryMethodId;
            existingBasket.PaymentIntentId = basket.PaymentIntentId;
            existingBasket.ShippingPrice = basket.ShippingPrice;

            await context.SaveChangesAsync();

            return basket;
        }
        public async Task DeleteBasketAsync(int basketId)
        {
            var foundBasket = await context.Baskets.FindAsync(basketId);

            if (foundBasket != null)
            {
                EntityEntry entityEntry = context.Entry(foundBasket);
                entityEntry.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Product>> GetBasketProducts(int basketId)
        {
            var basket = await context.Baskets.FindAsync(basketId);

            if (basket != null)
            {
                var products = new List<Product>();
                var basketItems = context.BasketItems.Where(i =>
                i.BasketId == basket.Id).ToList();
                foreach (var item in basketItems)
                {
                    var specs = new ProductsWithTypesAndBrandsSpecification(item.ProductId);
                    var product = await productRepo.GetByIdAsync(item.ProductId, specs);
                    products.Add(await context.Products.FindAsync(item.ProductId));
                }
                return products;
            }
            else
                return null;

        }
    }
}
