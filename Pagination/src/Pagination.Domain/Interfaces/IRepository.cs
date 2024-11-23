using Pagination.Domain.Models;

namespace Pagination.Domain;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<PagedResponseOffset<TEntity>> GetWithOffsetPagination(int pageNumber, int pageSize);
}
