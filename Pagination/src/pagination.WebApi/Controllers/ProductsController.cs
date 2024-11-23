using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pagination.WebApi.Dtos;
using Pagination.Domain;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, 
                                  IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet("GetWithOffsetPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWithOffsetPagination(int pageNumber =1, int pageSize =10)
        {
            if(pageNumber <= 0 || pageSize <=0)
                return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} size must be greater than 0.");
            
            var result = await _productService.GetWithOffsetPagination(pageNumber, pageSize);

            var pagedProductsDto = _mapper.Map<PagedResponseOffsetDto<ProductResultDto>>(result);

            return Ok(pagedProductsDto);
        }

        [HttpGet("GetWithKeysetPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWithKeysetPagination(int reference =0, int pageSize =10)
        {
            if(pageSize <=0)
                return BadRequest($"{nameof(pageSize)} size must be greater than 0.");

            var result = await _productService.GetWithKeysetPagination(reference, pageSize);

            var pagedProductsDto = _mapper.Map<PagedResponseKeysetDto<ProductResultDto>>(result);

            return Ok(pagedProductsDto);
        }
    }
}
