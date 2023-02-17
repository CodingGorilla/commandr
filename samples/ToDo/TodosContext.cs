using System;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo
{
    public class TodosContext : DbContext
    {
        public TodosContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoModel> Todos => Set<TodoModel>();
        public DbSet<TodoCategoryModel> TodoCategories => Set<TodoCategoryModel>();
        public DbSet<CategoryModel> Categories => Set<CategoryModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoModel>().HasKey(t => t.Id);
            modelBuilder.Entity<TodoModel>().HasMany(t => t.TodoCategories).WithOne(tc => tc.Todo).HasForeignKey(tc => tc.TodoId);

            modelBuilder.Entity<TodoCategoryModel>().HasKey(tc => new { tc.TodoId, tc.CategoryId });

            modelBuilder.Entity<CategoryModel>().HasKey(c => c.Id);
            modelBuilder.Entity<CategoryModel>().HasMany(c => c.Todos).WithOne(tc => tc.Category).HasForeignKey(tc => tc.CategoryId);
        }
    }
}