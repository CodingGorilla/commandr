using System;
using System.Threading.Tasks;

namespace Commandr.Samples.Jasper
{
    public class PingCommandHandler
    {
        public Task<string> Handle(PingCommand command)
        {
            return Task.FromResult("Pong!");
        }
    }

    [CommandRoute("/ping", "GET")]
    public class PingCommand : IRoutableCommand
    {
    }
}