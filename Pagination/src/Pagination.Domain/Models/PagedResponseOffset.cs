using System;

namespace Pagination.Domain.Models;

public record PagedResponseOffset<T>
{
    public PagedResponseOffset(List<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    }

    public List<T> Data { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalRecords { get; init; }
    public int TotalPages {get;init;} 
}
