using System;
using Microsoft.EntityFrameworkCore;
using Pagination.Domain;
using Pagination.Domain.Models;
using Pagination.Infrastructure.Context;

namespace Pagination.Infrastructure.Repositories;

// Generic Offset pagination with Generic Repository
public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly AppDbContext _context;

    protected Repository(AppDbContext context) {
        _context = context;
    }
    public virtual async Task<PagedResponseOffset<TEntity>> GetWithOffsetPagination(int pageNumber, int pageSize)
    {
        var totalRecords = await _context.Set<TEntity>().AsNoTracking().CountAsync();

        var entities = await _context.Set<TEntity>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var pagedResponse = new PagedResponseOffset<TEntity>(entities, pageNumber, pageSize,totalRecords);
        return pagedResponse;
    }
}
