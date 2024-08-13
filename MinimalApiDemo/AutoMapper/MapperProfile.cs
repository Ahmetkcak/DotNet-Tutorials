using AutoMapper;
using MinimalApiDemo.Models.DTOs;
using MinimalApiDemo.Models.Entities;

namespace MinimalApiDemo.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddRequestDTO, Product>();
        CreateMap<UpdateRequestDTO, Product>();
        CreateMap<Product, ResponseDto>();
        CreateMap<RegisterDTO,User>();
    }
}
