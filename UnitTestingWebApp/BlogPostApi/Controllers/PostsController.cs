using BlogPostApi.Models;
using BlogPostApi.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogPostApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _repository;
        private readonly ILogger<PostRepository> _logger;
        private readonly IValidator<Post> _validator;
        public PostsController(IPostRepository repository, ILogger<PostRepository> logger, IValidator<Post> validator )
        {
            _logger = logger;
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _repository.GetAllAsync();
            return Ok(posts);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _repository.GetByIdAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            if (post == null) { 
                throw new ArgumentNullException(nameof(post));
            }
            var validtionResult = await _validator.ValidateAsync(post);
            if (!validtionResult.IsValid)
            {
                throw new ValidationException(validtionResult.Errors);
            }

            var response = await _repository.CreateAsync(post);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] Post post)
        {
            var response = await _repository.UpdateAsync(post);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetPostsByCategory(int categoryId)
        {
            var posts = await _repository.GetPostsByCategoryAsync(categoryId);
            return Ok(posts);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery]string searchTerm)
        {
            var posts = await _repository.SearchPostsAsync(searchTerm);
            return Ok(posts);
        }

        [HttpGet("recent/{count}")]
        public async Task<IActionResult> GetRecentPosts(int count)
        {
            var posts = await _repository.GetRecentPostsAsync(count);
            return Ok(posts);
        }
        [HttpGet("exists/{id}")]
        public async Task<IActionResult> PostExists(Guid id)
        {
            var exists = await _repository.ExistsAsync(id);
            return Ok(exists);
        }

        [HttpGet("Statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _repository.GetPostStatisticsAsync();
            return Ok(statistics);
        }
    }
}
