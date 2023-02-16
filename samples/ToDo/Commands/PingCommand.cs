using System;
using Commandr;
using Commandr.Routing;

namespace ToDo.Commands
{
    [HttpGetCommand("/ping")]
    public class PingCommand
    {
        public Task<string> Invoke()
        {
            return Task.FromResult("pong");
        }
    }
}