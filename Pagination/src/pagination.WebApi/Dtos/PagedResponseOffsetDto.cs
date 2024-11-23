using System;

namespace pagination.WebApi.Dtos;

public class PagedResponseOffsetDto<T>
{
    public int PageNumber { get; init;}

    public int PageSize { get; init;}

    public int TotalPages { get; init;}

    public int TotalRecords { get; init;}

    public List<T> Data { get; init;}
}