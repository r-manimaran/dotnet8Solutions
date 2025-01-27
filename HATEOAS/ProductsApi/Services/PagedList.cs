﻿using Microsoft.EntityFrameworkCore;
using ProductsApi.Dtos;

namespace ProductsApi.Services;

public class PagedList<T>
{
    private PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public List<Link> Links { get; set; } = new();

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
      
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var pagedResponse =  new PagedList<T>(items, page, pageSize, totalCount);
        return pagedResponse;
    }
}
