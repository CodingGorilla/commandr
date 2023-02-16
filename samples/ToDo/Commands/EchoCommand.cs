using System;
using Commandr;
using Commandr.Binding;
using Commandr.Routing;

namespace ToDo.Commands
{
    [HttpPostCommand("/echo")]
    public class EchoCommandHandler
    {
        public async Task<string> Invoke([FromJsonBody] EchoCommand command)
        {
            await Task.Delay(command.DelayInMs);
            return command.Text;
        }
    }

    public class EchoCommand
    {
        public string Text { get; set; }
        public int DelayInMs { get; set; }
    }
}