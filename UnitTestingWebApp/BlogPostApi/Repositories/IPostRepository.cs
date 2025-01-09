using BlogPostApi.Dtos;
using BlogPostApi.Models;

namespace BlogPostApi.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<PostResponse>> GetAllAsync();
    Task<PostResponse> GetByIdAsync(Guid id);
    Task<PostResponse> CreateAsync(Post post);
    Task<PostResponse> UpdateAsync(Post post);
    Task DeleteAsync(Guid id);
}
