using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;
using System.Runtime.InteropServices;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly AppDbContext _dbContext;
        public ProductsController(ILogger<ProductsController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response =  await _dbContext.Products.ToListAsync();

            return Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateProduct([FromBody]Product product)
        {
            //newProduct = newProduct.CreatedDate = DateTime.UtcNow;
            var newProduct = new Product
            {
                Name = product.Name,
                CreatedDate = DateTime.Now
            };
            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
