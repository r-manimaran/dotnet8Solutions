using System;
using AutoMapper;
using pagination.WebApi.Dtos;
using Pagination.Domain;

namespace pagination.WebApi.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResultDto>();
    }
}
