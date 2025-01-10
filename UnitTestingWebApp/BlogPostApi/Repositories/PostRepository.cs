using BlogPostApi.Data;
using BlogPostApi.Dtos;
using BlogPostApi.Models;
using BlogPostApi.Validation;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApi.Repositories;

public class PostRepository : IPostRepository
{
    private readonly PostDbContext _dbContext;
    private readonly ILogger<PostRepository> _logger;
    private readonly IValidator<Post> _validator;

    public PostRepository(PostDbContext dbContext, ILogger<PostRepository> logger , IValidator<Post> validator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _validator = validator;
    }
    public async Task<PostResponse> CreateAsync(Post post)
    {
        if(post == null)
        {
            _logger.LogError("Post is null");
            throw new ArgumentNullException(nameof(post));
        }
        
        var validationResult = _validator.Validate(post);
        if(!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(", ",errors));
        }
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Post created");
        var response = post.Adapt<PostResponse>();
        return response;
    }

    public Task DeleteAsync(Guid id)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.Id == id);
        if(post is null)
        {
            _logger.LogError("Post not found");
            throw new Exception("Post not found");
        }
        _dbContext.Posts.Remove(post);
        _dbContext.SaveChanges();
        _logger.LogInformation("Post deleted");
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Posts.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PostResponse>> GetAllAsync()
    {
        var posts = await _dbContext.Posts.AsNoTracking()
                                          .Include(x=>x.Category)
                                          .ToListAsync();
        var response = posts.Adapt<IEnumerable<PostResponse>>();
        return response;
    }

    public async Task<PostResponse> GetByIdAsync(Guid id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if(post is null)
        {
            _logger.LogError("Post not found");
            throw new Exception("Post not found");
        }
        var response = post.Adapt<PostResponse>();
        return response;
    }

    public async Task<IEnumerable<PostResponse>> GetPostsByCategoryAsync(int categoryId)
    {
        var posts = await _dbContext.Posts
                            .AsNoTracking()
                            .Include(x=>x.Category)
                            .Where(p=>p.CategoryId == categoryId)
                            .ToListAsync();
        // Map to response
        var response = posts.Adapt<IEnumerable<PostResponse>>();
        return response;
    }

    public async Task<PostStatistics> GetPostStatisticsAsync()
    {
        var statistics = new PostStatistics
        {
            TotalPosts = await _dbContext.Posts.CountAsync(),
            TotalCategories = await _dbContext.Set<Category>().CountAsync(),
            MostRecentPost = await _dbContext.Posts.MaxAsync(p => p.CreatedDate),
            PostsPerCategory = await _dbContext.Posts
                                 .GroupBy(p => p.Category.Name)
                                 .Select(g => new { Category = g.Key, Count = g.Count() })
                                 .ToDictionaryAsync(x => x.Category, x => x.Count)

        };
        return statistics;
    }

    public async Task<IEnumerable<PostResponse>> GetRecentPostsAsync(int count)
    {
        if(count <= 0)
            throw new ArgumentException("Count must be greater than Zero", nameof(count));

        var posts = await _dbContext.Posts
                        .AsNoTracking()
                        .Include(x=>x.Category)
                        .OrderByDescending(p=>p.CreatedDate)
                        .Take(count)
                        .ToListAsync();
        var response = posts.Adapt<IEnumerable<PostResponse>>();
        return response;
    }

    public async Task<IEnumerable<PostResponse>> SearchPostsAsync(string searchTerm)
    {
       if(string.IsNullOrEmpty(searchTerm))
            throw new ArgumentException("Search term cannot be empty",nameof(searchTerm));

        var posts = await _dbContext.Posts
                      .AsNoTracking()
                      .Include(x => x.Category)
                      .Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm))
                      .ToListAsync();
        var response = posts.Adapt<IEnumerable<PostResponse>>();
        return response;
    }

    public async Task<PostResponse> UpdateAsync(Post post)
    {
        //update the existing post
        var existingPost = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
        if(existingPost is null)
        {
            _logger.LogError("Post not found");
            throw new Exception("Post not found");
        }
        // use Mapster to map the post to the existing post
        post.Adapt(existingPost);
        _dbContext.Posts.Update(existingPost);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Post updated");
        var response = existingPost.Adapt<PostResponse>();
        return response;
    }
}
