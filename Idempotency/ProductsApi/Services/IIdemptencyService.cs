namespace ProductsApi.Services;

public interface IIdemptencyService
{
    Task<bool> RequestExistsAsync(Guid requestId);
    Task CreateRequestAsync(Guid requestId, string name);
}
