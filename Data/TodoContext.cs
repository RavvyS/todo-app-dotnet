using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    /// <summary>
    /// Database context - handles all database operations
    /// This is where Entity Framework connects to your database
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet represents the 'todos' table in database
        /// Each TodoItem becomes a row in this table
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// Configure database schema and relationships
        /// This method runs when EF creates the database
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TodoItem entity
            modelBuilder.Entity<TodoItem>(entity =>
            {
                // Set primary key
                entity.HasKey(e => e.Id);

                // Configure Title property constraints
                entity.Property(e => e.Title)
                      .IsRequired()                 // NOT NULL in database
                      .HasMaxLength(200);           // VARCHAR(200)

                // Configure Description property
                entity.Property(e => e.Description)
                      .HasMaxLength(1000);          // VARCHAR(1000), nullable

                // Configure datetime properties
                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("datetime('now')"); // SQLite function

                entity.Property(e => e.UpdatedAt)
                      .IsRequired();

                // Add database indexes for better query performance
                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("IX_TodoItems_CreatedAt");

                entity.HasIndex(e => e.IsCompleted)
                      .HasDatabaseName("IX_TodoItems_IsCompleted");
            });
        }
    }
}