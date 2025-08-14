using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    /// <summary>
    /// Service class containing all business logic
    /// This keeps controllers thin and focused on HTTP concerns
    /// </summary>
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoService> _logger;

        public TodoService(TodoContext context, IMapper mapper, ILogger<TodoService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all todos, sorted by creation date (newest first)
        /// </summary>
        public async Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all todos");
                
                var todos = await _context.TodoItems
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all todos");
                throw;
            }
        }

        /// <summary>
        /// Get a specific todo by ID
        /// </summary>
        public async Task<TodoResponseDto?> GetTodoByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching todo with ID: {TodoId}", id);
                
                var todo = await _context.TodoItems.FindAsync(id);
                
                if (todo == null)
                {
                    _logger.LogWarning("Todo with ID {TodoId} not found", id);
                    return null;
                }

                return _mapper.Map<TodoResponseDto>(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching todo with ID: {TodoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Create a new todo item
        /// </summary>
        public async Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new todo with title: {Title}", createDto.Title);
                
                // Map DTO to entity
                var todoItem = _mapper.Map<TodoItem>(createDto);
                
                // Set timestamps
                todoItem.CreatedAt = DateTime.UtcNow;
                todoItem.UpdatedAt = DateTime.UtcNow;

                // Add to database
                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created todo with ID: {TodoId}", todoItem.Id);
                
                return _mapper.Map<TodoResponseDto>(todoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating todo");
                throw;
            }
        }

        /// <summary>
        /// Update an existing todo item
        /// </summary>
        public async Task<TodoResponseDto?> UpdateTodoAsync(int id, UpdateTodoDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating todo with ID: {TodoId}", id);
                
                var existingTodo = await _context.TodoItems.FindAsync(id);
                
                if (existingTodo == null)
                {
                    _logger.LogWarning("Todo with ID {TodoId} not found for update", id);
                    return null;
                }

                // Update properties
                existingTodo.Title = updateDto.Title;
                existingTodo.Description = updateDto.Description;
                existingTodo.IsCompleted = updateDto.IsCompleted;
                existingTodo.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully updated todo with ID: {TodoId}", id);
                
                return _mapper.Map<TodoResponseDto>(existingTodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating todo with ID: {TodoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Toggle completion status of a todo
        /// </summary>
        public async Task<TodoResponseDto?> ToggleTodoAsync(int id)
        {
            try
            {
                _logger.LogInformation("Toggling todo with ID: {TodoId}", id);
                
                var todo = await _context.TodoItems.FindAsync(id);
                
                if (todo == null)
                {
                    _logger.LogWarning("Todo with ID {TodoId} not found for toggle", id);
                    return null;
                }

                todo.IsCompleted = !todo.IsCompleted;
                todo.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully toggled todo with ID: {TodoId} to {Status}", 
                    id, todo.IsCompleted ? "completed" : "pending");
                
                return _mapper.Map<TodoResponseDto>(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling todo with ID: {TodoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Delete a todo item
        /// </summary>
        public async Task<bool> DeleteTodoAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting todo with ID: {TodoId}", id);
                
                var todo = await _context.TodoItems.FindAsync(id);
                
                if (todo == null)
                {
                    _logger.LogWarning("Todo with ID {TodoId} not found for deletion", id);
                    return false;
                }

                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Successfully deleted todo with ID: {TodoId}", id);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting todo with ID: {TodoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Get todos filtered by completion status
        /// </summary>
        public async Task<IEnumerable<TodoResponseDto>> GetTodosByStatusAsync(bool isCompleted)
        {
            try
            {
                _logger.LogInformation("Fetching todos with completion status: {Status}", isCompleted);
                
                var todos = await _context.TodoItems
                    .Where(t => t.IsCompleted == isCompleted)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching todos by status: {Status}", isCompleted);
                throw;
            }
        }
    }
}