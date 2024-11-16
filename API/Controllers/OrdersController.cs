using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        string email;
        IConfiguration _config;

        public OrdersController(IOrderService orderService, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUser> user, IConfiguration config)
        {
            _orderService = orderService;
            _mapper = mapper;
            _userManager = user;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderDto)
        {
            string userId = TokenExtractor.GetUserId(_config, HttpContext);
            email = _userManager.Users.FirstOrDefault(u => u.Id == userId).Email;

            var address = _mapper.Map<AddressDto, Core.Entities.OrderAggregate.Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem Creating an order"));
            }

            return Ok(order);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {

            // check
            string userId = TokenExtractor.GetUserId(_config, HttpContext);

            //email = HttpContext.User?.FindFirstValue(ClaimTypes.Email);

            email = _userManager.Users.FirstOrDefault(u => u.Id == userId).Email;

            // hard code (to be removed)
            //email = "Than@furnitica.com";
            ///////


            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            string userId = TokenExtractor.GetUserId(_config, HttpContext);
            email = _userManager.Users.FirstOrDefault(u => u.Id == userId).Email;

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodAsync());
        }


        [HttpPost("{id}")]
        public async Task<ActionResult<Basket>> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            string userId = TokenExtractor.GetUserId(_config, HttpContext);
            email = _userManager.Users.FirstOrDefault(u => u.Id == userId).Email;

            //var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var updatedOrder = _mapper.Map<OrderDto, Order>(orderDto);

            var orderToBeUpdated = await _orderService.GetOrderByIdAsync(id, email);

            if (orderToBeUpdated == null)
            {
                return BadRequest(new ApiResponse(400));
            }
            else
            {
                orderToBeUpdated = updatedOrder;
                await _orderService.UpdateOrder(id, orderToBeUpdated);
                return Ok();
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            string userId = TokenExtractor.GetUserId(_config, HttpContext);
            email = _userManager.Users.FirstOrDefault(u => u.Id == userId).Email;

            try
            {
                await _orderService.CancelOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404, "No Order with this ID"));
            }
        }


    }
}
