using Pagination.Domain.Models;

namespace Pagination.Domain;

public interface IOrderService
{
    Task<PagedResponseOffset<Order>> GetResponseOffset(int pageNumber, int pageSize);
}
