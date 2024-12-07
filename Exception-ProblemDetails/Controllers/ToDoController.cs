using Exception_ProblemDetails.Models;
using Exception_ProblemDetails.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exception_ProblemDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _todoService;

        public ToDoController(IToDoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var todo = await _todoService.GetToDoAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(ToDoItem newItem)
        {
            var todo = await _todoService.CreateToDoAsync(newItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todo.Id }, todo);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todo = await _todoService.DeleteToDoAsync(id);
          
            return Ok(todo);
        }
    }
}
