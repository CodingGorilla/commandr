using System;

namespace ToDo
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Notes { get; set; }
        public string Priority { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}