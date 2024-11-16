using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminOrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        string email;
        public AdminOrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<OrderToReturnDto>>>> GetAllOrders([FromQuery] OrderSpecParams orderParams)
        {
            var specs = new OrdersSpecification(orderParams);

            var countSpecs = new OrdersWithFiltersCount(orderParams);

            var totalItems = await _orderService.CountAsync(countSpecs);

            var orders = await _orderService.GetAllOrdersAsync(specs);

            var data = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);

            data = data.Where(o => o.Total >= orderParams.SubTotalFrom && o.Total <= orderParams.SubTotalTo).ToList();

            var response = new Pagination<OrderToReturnDto>
            {
                Count = totalItems,
                PageSize = orderParams.PageSize,
                PageIndex = orderParams.PageIndex,
                Data = data
            };

            return Ok(response);

        }

        [HttpGet("maxPrice")]
        public async Task<ActionResult<decimal>> GetMaxPrice()
        {
            var specs = new OrdersSpecification();
            var result = await _orderService.GetAllOrdersAsync();
            var maxPrice = result.Max(p => p.Subtotal);

            return Ok(maxPrice);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ConfirmOrder(int id)
        {

            var order = await _orderService.GetOrderById(id);

            order.Status = OrderStatus.Confirmed;

            await _orderService.confirmOrder(id, order);

            return Ok();

        }

        [HttpPut("confirmAllOrders")]
        public async Task<ActionResult> ConfirmAllOrder()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            foreach(var order in orders)
            {
                order.Status = OrderStatus.Confirmed;
                await _orderService.confirmOrder(order.Id, order);
            }
            return Ok();
        }
    }

}