using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(TodoContext context, ILogger<TodoItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/todoitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            try
            {
                var todos = await _context.TodoItems
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching todos");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/todoitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            try
            {
                var todo = await _context.TodoItems.FindAsync(id);
                if (todo == null)
                {
                    return NotFound();
                }
                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/todoitems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todoItem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(todoItem.Title))
                {
                    return BadRequest("Title is required");
                }

                todoItem.CreatedAt = DateTime.UtcNow;
                todoItem.UpdatedAt = DateTime.UtcNow;

                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/todoitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, TodoItem todoItem)
        {
            try
            {
                if (id != todoItem.Id)
                {
                    return BadRequest("ID mismatch");
                }

                var existingTodo = await _context.TodoItems.FindAsync(id);
                if (existingTodo == null)
                {
                    return NotFound();
                }

                existingTodo.Title = todoItem.Title;
                existingTodo.Description = todoItem.Description;
                existingTodo.IsCompleted = todoItem.IsCompleted;
                existingTodo.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // PATCH: api/todoitems/5/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult<TodoItem>> ToggleTodoItem(int id)
        {
            try
            {
                var todo = await _context.TodoItems.FindAsync(id);
                if (todo == null)
                {
                    return NotFound();
                }

                todo.IsCompleted = !todo.IsCompleted;
                todo.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/todoitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            try
            {
                var todo = await _context.TodoItems.FindAsync(id);
                if (todo == null)
                {
                    return NotFound();
                }

                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}