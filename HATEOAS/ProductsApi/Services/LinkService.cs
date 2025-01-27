using ProductsApi.Dtos;
using System.Runtime.CompilerServices;

namespace ProductsApi.Services;

internal sealed class LinkService : ILinkService
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }
    public Link Generate(string endpointName, object? routeValues, string rel, string method)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }

        var uri = _linkGenerator.GetUriByName(httpContext, endpointName, routeValues);
        if (uri == null)
        {
            throw new InvalidOperationException("Generated URI is null");
        }

        return new Link(uri, rel, method);
    }
}
