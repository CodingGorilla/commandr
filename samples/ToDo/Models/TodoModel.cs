using System;

namespace ToDo.Models
{
    public class TodoModel
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Notes { get; set; }
    }
}