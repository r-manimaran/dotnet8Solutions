using System;
using Pagination.Domain;
using Pagination.Infrastructure.Context;

namespace Pagination.Infrastructure.Repositories;

// Example using a generic offset Pagination from a generic Repository
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
        
    }
}
