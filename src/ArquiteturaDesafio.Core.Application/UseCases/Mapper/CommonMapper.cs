using AutoMapper;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;
using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery;
using ArquiteturaDesafio.Application.UseCases.Commands.Customer.CreateCustomer;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetCustomerById;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetProductById;
using ArquiteturaDesafio.Application.UseCases.Commands.Product.CreateProduct;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.CreateSale;
using Ambev.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Application.UseCases.Queries.GetOrderById;
using ArquiteturaDesafio.Application.UseCases.Commands.Order.UpdateSale;

namespace ArquiteturaDesafio.Core.Application.UseCases.Mapper
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            // Customer
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<Customer, GetCustomerByIdResponse>();
            
            //Adress
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            //Adress
            CreateMap<InfoContact, InfoContactDTO>();
            CreateMap<InfoContactDTO, InfoContact>();

            //
            CreateMap<GeolocationDto, Geolocation>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Lat))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Long));

            CreateMap<Geolocation, GeolocationDto>()
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Longitude));

            //User
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, CreateUserResponse>();
            CreateMap<DeleteUserRequest, User>();
            CreateMap<User, DeleteUserRequest>();
            CreateMap<User, GetUsersQueryResponse>();
            CreateMap<User, UserDTO>();
            CreateMap<User, UpdateUserResponse>();
            CreateMap<UserDTO, UpdateUserResponse>();

            //Product
            
            CreateMap<ProductBaseDTO, ProductBaseDTO> ();
            CreateMap<CreateProductRequest, Product > ();
            CreateMap<Product, GetProductByIdResponse>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Product, ProductBaseDTO>();

            // Order
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<OrderBaseDTO, Order>();
            CreateMap<OrderItemBaseDTO, OrderItem>();
            CreateMap<OrderRead, SaleWithDetaislsDTO>();
            CreateMap<OrderRead, GetOrderByIdResponse>();
            CreateMap<OrderRead, OrderReadDTO>();
            CreateMap<OrderItemRead, OrderItemReadDTO>();
            CreateMap<CustomerOrder, CustomerOrderDTO>();
            CreateMap<UpdateSaleItemRequest, OrderItem>();
            

        }
    }
}
