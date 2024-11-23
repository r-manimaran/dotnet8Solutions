﻿using Pagination.Domain.Models;

namespace Pagination.Domain;

public interface IProductRepository
{
    Task<PagedResponseOffset<Product>> GetWithOffsetPagination(int pageNumber, int pageSize);
    Task<PagedResponseKeyset<Product>> GetWithKeysetPagination(int reference, int pageSize);
}
