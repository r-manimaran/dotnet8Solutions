using System;
using AutoMapper;
using Pagination.Domain;
using pagination.WebApi.Dtos;

namespace pagination.WebApi.Mappers;

public class OrderProfile: Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderResultDto>();
    }
}
