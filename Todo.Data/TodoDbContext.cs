using Microsoft.EntityFrameworkCore;
using Todo.Core.Models.Sql;

namespace Todo.Data
{
    /// <summary>
    /// EF Core database context for the application
    /// </summary>
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<TodoList> TodoLists { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<Label> Labels { get; set; }

        /// <summary>
        /// Apply explicit configurations on model creations.
        /// Also contains database seeding (should not be used for production applications)
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // should not allow duplicate usernames in the database
            modelBuilder.Entity<User>().HasIndex(x => new { x.Username }).IsUnique();

            // all todo items should be deleted when the parent list is deleted
            modelBuilder.Entity<TodoItem>()
                .HasOne<TodoList>()
                .WithMany(t => t.Items)
                .OnDelete(DeleteBehavior.Cascade);

            // cascade deletion for TodoItems => Labels
            modelBuilder.Entity<Label>()
                .HasOne<TodoItem>()
                .WithMany(t => t.Labels)
                .OnDelete(DeleteBehavior.Cascade);

            // cannot apply cascade delete on both items and list due to multiple cascade paths
            // if a list is deleted, the labels for the list must be deleted first by code
        }
    }
}
