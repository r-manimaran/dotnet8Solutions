using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Models;

namespace ProductsApi.Services
{
    internal sealed class IdempotencyService : IIdemptencyService
    {
        private readonly AppDbContext _dbContext;

        public IdempotencyService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateRequestAsync(Guid requestId, string name)
        {
            var idempotentRequest = new IdempotentRequest
            {
                Id = requestId,
                Name = name,
                CreatedOnUtc = DateTime.UtcNow,
            };
            _dbContext.Add(idempotentRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RequestExistsAsync(Guid requestId)
        {
            return await _dbContext.Set<IdempotentRequest>().AnyAsync(r=>r.Id == requestId);
        }
    }
}
