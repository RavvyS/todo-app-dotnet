using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    /// <summary>
    /// Represents a Todo item in the database
    /// This is your core entity that maps to the database table
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// Primary key - auto-incremented by database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the todo item (required, max 200 chars)
        /// </summary>
        [Required]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Optional description (max 1000 chars)
        /// </summary>
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Completion status - defaults to false
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// When the item was created (auto-set)
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the item was last updated (auto-updated)
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}