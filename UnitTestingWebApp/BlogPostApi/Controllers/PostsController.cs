using BlogPostApi.Models;
using BlogPostApi.Repositories;
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
        public PostsController(IPostRepository repository, ILogger<PostRepository> logger)
        {
            _logger = logger;
            _repository = repository;
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
            if (post == null) return BadRequest("Payload is required.");

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
    }
}
