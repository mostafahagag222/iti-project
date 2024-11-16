using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        private readonly IConfiguration config;

        public MappingProfiles()
        {

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.Pictures, o => o.MapFrom(s => s.Pictures.Select(x => x.ImageUrl)));


            CreateMap<AppUser, UserDto>()
                .ForMember(d => d.username, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));

            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName));

            CreateMap<ProductType, ProductTypeDto>().ReverseMap();

            CreateMap<ProductBrand, ProductBrandDto>().ReverseMap();

            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.OrderItems, o => o.MapFrom(s => s.OrderItems.ToList()))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Subtotal + s.DeliveryMethod.Price));

            CreateMap<OrderToReturnDto, Order>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName));
        }
    }
}
