using System;
using AutoMapper;
using pagination.WebApi.Dtos;
using Pagination.Domain.Models;

namespace pagination.WebApi.Mappers;

public class PagedResponseOffsetProfile : Profile
{
    public PagedResponseOffsetProfile()
    {
        CreateMap(typeof(PagedResponseOffset<>), typeof(PagedResponseOffsetDto<>));
    }
}
