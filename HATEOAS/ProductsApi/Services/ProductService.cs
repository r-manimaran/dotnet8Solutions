using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Dtos;
using ProductsApi.Models;

namespace ProductsApi.Services
{
    public class ProductService : IProductService
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILinkService _linkService;

        public ProductService(AppDbContext appDbContext, LinkGenerator linkGenerator, ILinkService linkService, IHttpContextAccessor httpContext)
        {
            _linkGenerator = linkGenerator;
            _appDbContext = appDbContext;
            _httpContext = httpContext;
            _linkService = linkService;
        }

        public Task<ProductResponse> CreateProductAsync(ProductRequest productRequest)
        {
            Guid newGuid = Guid.NewGuid();
            _appDbContext.Products.Add(new Product
            {
                Id = newGuid,
                Name = productRequest.Name,
                Sku = productRequest.Sku,
                Currency = productRequest.Currency,
                Amount = productRequest.Amount
            });
            _appDbContext.SaveChanges();
            return Task.FromResult(new ProductResponse(newGuid, productRequest.Name, productRequest.Sku, productRequest.Currency, productRequest.Amount));
        }

        public Task DeleteProductAsync(Guid id)
        {
            var product = _appDbContext.Products.Find(id);
            if (product != null)
            {
                _appDbContext.Products.Remove(product);
                _appDbContext.SaveChanges();
            }
            return Task.CompletedTask;
        }

        public async Task<ProductResponse> GetProductAsync(Guid id)
        {
            var product = await _appDbContext.Products.FindAsync(id);

            if (product != null)
            {
                ProductResponse productResponse = new ProductResponse(product.Id, product.Name, product.Sku, product.Currency, product.Amount);

                AddLinkForProduct(productResponse);

                return productResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ProductResponse>> GetProductsAsync()
        {
            var products = _appDbContext.Products.ToList();

            return products.Select(
                        product =>
                        new ProductResponse(product.Id,
                                            product.Name,
                                            product.Sku,
                                            product.Currency,
                                            product.Amount)).ToList();

        }

        public async Task<PagedList<ProductResponse>> GetProductsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page, int pageSize)
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Sku.Contains(searchTerm));
            }

            // Apply Sorting
            query = ApplySorting(query, sortColumn, sortOrder);

            var productResponseQuery = query.Select(p=> new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Sku,
                    p.Currency,
                    p.Amount));

            //Execute Query         
            var items = await PagedList<ProductResponse>.CreateAsync(productResponseQuery, page, pageSize);

            AddLinksForPagedProducts(items, searchTerm, sortColumn, sortOrder, page, pageSize);
            return items;

        }


        public Task<ProductResponse> UpdateProductAsync(Guid id, ProductRequest productRequest)
        {
            var existingProduct = _appDbContext.Products.Find(id);
            if (existingProduct != null)
            {
                existingProduct.Name = productRequest.Name;
                existingProduct.Sku = productRequest.Sku;
                existingProduct.Currency = productRequest.Currency;
                existingProduct.Amount = productRequest.Amount;
                _appDbContext.SaveChanges();
            }
            return Task.FromResult(new ProductResponse(id, productRequest.Name, productRequest.Sku, productRequest.Currency, productRequest.Amount));
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortColumn, string? sortOrder)
        {
            var isDescending = sortOrder?.ToLower() == "desc";

            query = sortColumn?.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
                "sku" => isDescending
                    ? query.OrderByDescending(p => p.Sku)
                    : query.OrderBy(p => p.Sku),
                "amount" => isDescending
                    ? query.OrderByDescending(p => p.Amount)
                    : query.OrderBy(p => p.Amount),
                _ => query.OrderBy(p => p.Id) // Default sorting
            };

            return query;
        }

        private void AddLinkForProduct(ProductResponse productResponse)
        {
            var context = _httpContext.HttpContext;

            productResponse.Links.Add(
                _linkService.Generate("GetProduct",
                                      new { id = productResponse.Id },
                                      "self", "GET"));

            productResponse.Links.Add(
               _linkService.Generate("GetProducts",
                                     null,
                                     "products", "GET"));

            productResponse.Links.Add(
               _linkService.Generate("UpdateProduct",
                                     new { id = productResponse.Id },
                                     "update-product", "PUT"));
            productResponse.Links.Add(
               _linkService.Generate("DeleteProduct",
                                     new { id = productResponse.Id },
                                     "delete-product", "DELETE"));


        }

        private void AddLinksForPagedProducts(PagedList<ProductResponse> products, string? searchTerm,
                                    string? sortColumn,
                                    string? sortOrder,
                                    int page,
                                    int pageSize)
        {
            var context = _httpContext.HttpContext;

            // Create route values dictionary
            var routeValues = new Dictionary<string, object?>
            {
                { "searchTerm", searchTerm },
                { "sortColumn", sortColumn },
                { "sortOrder", sortOrder },
                { "pageSize", pageSize }
            };

            // Add current page link
            routeValues["page"] = page;
            products.Links.Add(new Link(
                _linkGenerator.GetUriByName(context, "GetProducts", routeValues),
                "self",
                "GET"));

            // Add first page link
            routeValues["page"] = 1;
            products.Links.Add(new Link(
                _linkGenerator.GetUriByName(context, "GetProducts", routeValues),
                "first",
                "GET"));
            
            // Next Page link
            if (products.HasNextPage)
            {
                routeValues["page"] = page + 1;
                products.Links.Add(
                    _linkService.Generate(
                        "GetProducts",
                        routeValues,
                        "next-page",
                        "GET"));
            }

            // Previous page link
            if (products.HasPreviousPage)
            {
                routeValues["page"] = page - 1;
                products.Links.Add(
                    _linkService.Generate(
                        "GetProducts",
                        routeValues,
                        "previous-page",
                        "GET"));
            }

            // Add last page link
            routeValues["page"] = products.TotalPages;
            products.Links.Add(new Link(
                _linkGenerator.GetUriByName(context, "GetProducts", routeValues),
                "last",
                "GET"));


        }
    }
}
