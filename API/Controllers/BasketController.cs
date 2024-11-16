using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasketById(string userId)
        {
            var basket = await basketRepository.GetBasketAsync(userId);
            if (basket == null)
                return BadRequest(new ApiResponse(400));

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<Basket>> UpdateBasket([FromBody]Basket basket)
        {
            var updatedBasket = await basketRepository.UpdateBasketAsync(basket);
            
            if (updatedBasket != null)
                return Ok(updatedBasket);
            else
                return BadRequest(new ApiResponse(400));
        }

        [HttpGet]
        [Route("/api/Basket/{basketId:int}")]
        public async Task<ActionResult<List<Product>>> GetBasketProducts(int basketId)
        {
            var products = await basketRepository.GetBasketProducts(basketId);
            if (products != null)
            {
                var productDtos = mapper.Map<List<Product>, List<ProductDto>>(products);           
                return Ok(productDtos);
            }
            else
                return BadRequest(new ApiResponse(400));
        }

        [HttpDelete]
        public async Task DeleteBasket(int id)
        {
            await basketRepository.DeleteBasketAsync(id);
        }
    }
}
