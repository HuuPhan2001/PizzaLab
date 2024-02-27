using AutoMapper;
using BusinessObject.Models;
using PizzaLabApi.Models;

namespace PizzaLabApi.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();


            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailDTO>();

            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();


            CreateMap<ReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>();


            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();

        }
    }
}
