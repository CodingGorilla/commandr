using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Commandr.Samples.Mediatr
{
    [CommandRoute("/ping", "GET")]
    public class PingCommand : IRequest<string>, IRoutableCommand
    {
        
    }

    public class PingCommandHandler : IRequestHandler<PingCommand, string>
    {
        public Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Pong!");
        }
    }
}