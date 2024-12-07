using System;
using Exception_ProblemDetails.Models;

namespace Exception_ProblemDetails.Services;

public interface IToDoService
{
    Task<ToDoItem> GetToDoAsync(int id);
    Task<ToDoItem> CreateToDoAsync(ToDoItem item);
    Task<bool> DeleteToDoAsync(int id);
}
