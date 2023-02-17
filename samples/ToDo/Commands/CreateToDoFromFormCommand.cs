using System;
using Commandr.Binding;
using Commandr.Routing;
using ToDo.Models;

namespace ToDo.Commands
{
    [HttpPostCommand("/todos/form", typeof(Todo))]
    public class CreateToDoFromFormCommand
    {
        public Task<TodoModel> InvokeAsync([FromFormField("label")] string label, [FromFormField("notes")] string notes)
        {
            return Task.FromResult(new TodoModel() { Label = label, Notes = notes, Id = Guid.NewGuid() });
        }
    }
}