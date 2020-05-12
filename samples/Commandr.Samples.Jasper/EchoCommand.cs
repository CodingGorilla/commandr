using System;
using Commandr.Binding;
using Commandr.Results;
using Microsoft.AspNetCore.Authorization;

namespace Commandr.Samples.Jasper
{
    [CommandRoute("/echo", "GET")]
    [FromJsonBody]
    public class EchoCommand : IRoutableCommand
    {
        public string Message { get; set; }
    }

    public class EchoCommandHandler
    {
        public ICommandResult Handle(EchoCommand command)
        {
            return new DefaultCommandResult($"ECHO: {command.Message}");
        }
    }
}