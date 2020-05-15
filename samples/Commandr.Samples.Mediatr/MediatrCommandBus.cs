using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr.Samples.Mediatr
{
    public class MediatrCommandBus : ICommandBus
    {
        private readonly IMediator _mediator;

        public MediatrCommandBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<object> InvokeCommandAsync(IRoutableCommand command)
        {
            return _mediator.Send(command);
        }
    }

    public static class MediatrCommandrConfigBuilderExtensions
    {
        public static void UseMediatr(this CommandrConfigurationBuilder configBuilder)
        {
            configBuilder.ServiceCollection.AddTransient<ICommandBus, MediatrCommandBus>();
        }
    }
}