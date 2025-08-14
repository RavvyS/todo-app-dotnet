using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Services
{
    /// <summary>
    /// Interface defining business logic operations
    /// This separates business logic from API controllers
    /// Makes code testable and maintainable
    /// </summary>
    public interface ITodoService
    {
        Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync();
        Task<TodoResponseDto?> GetTodoByIdAsync(int id);
        Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto createDto);
        Task<TodoResponseDto?> UpdateTodoAsync(int id, UpdateTodoDto updateDto);
        Task<TodoResponseDto?> ToggleTodoAsync(int id);
        Task<bool> DeleteTodoAsync(int id);
        Task<IEnumerable<TodoResponseDto>> GetTodosByStatusAsync(bool isCompleted);
    }
}