using Pagination.Domain.Models;

namespace Pagination.Domain;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<PagedResponseOffset<Order>> GetResponseOffset(int pageNumber, int pageSize)
    {
       return await _orderRepository.GetWithOffsetPagination(pageNumber, pageSize);
    }
}
