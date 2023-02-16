using System;
using Commandr;
using Commandr.Binding;
using Commandr.Routing;

namespace ToDo.Commands
{
    [HttpPostCommand("/todos")]
    public class CreateToDoFromFormCommand
    {
        public Task<Todo> InvokeAsync([FromFormField("label")] string label, [FromFormField("notes")] string notes)
        {
            return Task.FromResult(new Todo { Label = label, Notes = notes, Id = Guid.NewGuid() });
        }
    }
}