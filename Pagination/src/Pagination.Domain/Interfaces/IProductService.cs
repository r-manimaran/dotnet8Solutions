﻿using Pagination.Domain.Models;

namespace Pagination.Domain;

public interface IProductService
{
    Task<PagedResponseOffset<Product>> GetWithOffsetPagination(int pageNumber, int pageSize);
    Task<PagedResponseKeyset<Product>> GetWithKeysetPagination(int reference, int pageSize);
}
