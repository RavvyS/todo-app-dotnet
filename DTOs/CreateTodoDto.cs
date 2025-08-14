using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating new todos
    /// Separates API input from database entity
    /// </summary>
    public class CreateTodoDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO for updating existing todos
    /// </summary>
    public class UpdateTodoDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }

    /// <summary>
    /// DTO for API responses - controls what data is sent to client
    /// </summary>
    public class TodoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}