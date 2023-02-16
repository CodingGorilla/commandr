using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commandr.Samples.Mediatr
{
    [HttpGet("/"ping)]
    [CommandRoute("/ping", "GET")]
    public class PingCommand : IRequest<string>, IRoutableCommand
    {
        
    }

    public class PingCommandHandler : IRequestHandler<PingCommand, string>
    {
        public Task<string> Handle([FromBody]PingCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Pong!");
        }
    }
}