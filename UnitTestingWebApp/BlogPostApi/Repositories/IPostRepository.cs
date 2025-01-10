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

    // New methods
    Task<IEnumerable<PostResponse>> GetPostsByCategoryAsync(int categoryId);
    Task<IEnumerable<PostResponse>> SearchPostsAsync(string searchTerm);
    Task<IEnumerable<PostResponse>> GetRecentPostsAsync(int count);
    Task<bool> ExistsAsync(Guid id);
    Task<PostStatistics> GetPostStatisticsAsync();
}
