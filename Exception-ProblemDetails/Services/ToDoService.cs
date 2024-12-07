
using System;
using Exception_ProblemDetails.Models;

namespace Exception_ProblemDetails.Services;
public class ToDoService: IToDoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ToDoService> _logger;

    public ToDoService(AppDbContext context, 
                       ILogger<ToDoService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ToDoItem> CreateToDoAsync(ToDoItem item)
    {
       await _context.ToDoItems.AddAsync(item);
       _context.SaveChanges();
       return item;
    }

    public Task<bool> DeleteToDoAsync(int id)
    {
        var item =  _context.ToDoItems.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            _context.ToDoItems.Remove(item);
            _context.SaveChanges();
        }
        else
        {
            throw new ApplicationException("Item not found");  
        }
        return Task.FromResult(true);       
    }

    public async Task<ToDoItem> GetToDoAsync(int id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null)
        {
            throw new ApplicationException("Item not found");
        }
        return item;
    }
}