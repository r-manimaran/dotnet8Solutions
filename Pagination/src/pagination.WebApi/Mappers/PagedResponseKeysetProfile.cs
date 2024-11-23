using System;
using AutoMapper;
using pagination.WebApi.Dtos;
using Pagination.Domain.Models;

namespace pagination.WebApi.Mappers;

public class PagedResponseKeysetProfile : Profile
{
    public PagedResponseKeysetProfile()
    {
        CreateMap(typeof(PagedResponseKeyset<>), typeof(PagedResponseKeysetDto<>));           
    }
}
