using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pagination.WebApi.Dtos;
using Pagination.Domain;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersController(
                         IMapper mapper,
                         IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet("GetWithGenericOffsetPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWithGenericOffsetPagination(int pageNumber = 1, int pageSize = 10)
        {
            // validation
            if( pageNumber < 1 || pageSize < 1)
                return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} size must be greater than 0.");

            var result = await _orderService.GetResponseOffset(pageNumber, pageSize);
            var pagedOrdersDto = _mapper.Map<PagedResponseOffsetDto<OrderResultDto>>(result);

            return Ok(pagedOrdersDto);
        }
    }
}
