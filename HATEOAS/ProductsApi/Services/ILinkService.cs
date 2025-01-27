using ProductsApi.Dtos;

namespace ProductsApi.Services;

public interface ILinkService
{
    Link Generate(string endpointName, object? routeValues, string rel, string method);
}
