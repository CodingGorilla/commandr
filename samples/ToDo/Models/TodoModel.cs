using System;

namespace ToDo.Models
{
    public class TodoModel
    {
        public Guid Id { get; set; }
        public required string Label { get; set; }
        public string? Notes { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;

        public ICollection<TodoCategoryModel> TodoCategories { get; set; }
    }

    public enum Priority
    {
        Normal,
        High,
        Low,
        Trivial
    }
}