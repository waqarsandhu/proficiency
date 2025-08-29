using AutoMapper;
using PM.Application.Dto;
using PM.Common.Dto;
using PM.EntityFrameworkCore.Entities;

namespace PM.Application.Mappings
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateOrUpdateProductDto>().ReverseMap();
        }
    }
}
