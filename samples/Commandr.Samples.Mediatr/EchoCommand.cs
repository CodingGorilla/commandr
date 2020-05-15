using System;
using System.Threading;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Results;
using MediatR;

namespace Commandr.Samples.Mediatr
{
    [CommandRoute("/echo", "POST")]
    [FromJsonBody]
    public class EchoCommand : IRequest<ICommandResult>, IRoutableCommand
    {
        public string Message { get; set; }
    }

    public class EchoCommandHandler : IRequestHandler<EchoCommand, ICommandResult>
    {
        public Task<ICommandResult> Handle(EchoCommand command, CancellationToken token)
        {
            return Task.FromResult<ICommandResult>(new DefaultCommandResult($"ECHO: {command.Message}"));
        }
    }
}