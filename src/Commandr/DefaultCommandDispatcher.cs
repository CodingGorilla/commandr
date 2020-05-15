using System;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr
{
    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        private readonly CommandBinder _commandBinder;

        public DefaultCommandDispatcher(Type commandType)
        {
            _commandBinder = new CommandBinder(commandType);
        }

        public async Task Dispatch(HttpContext context)
        {
            var commandBus = context.RequestServices.GetRequiredService<ICommandBus>();

            var command = await _commandBinder.GenerateCommandAsync(context.Request).ConfigureAwait(false);
            if(!(command is IRoutableCommand routableCommand))
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("The route resulted in a command which could not be executed").ConfigureAwait(false);
                return;
            }

            var result = await commandBus.InvokeCommandAsync(routableCommand).ConfigureAwait(false);

            switch(result)
            {
                case ICommandResult commandResult:
                    await commandResult.ExecuteAsync(context).ConfigureAwait(false);
                    return;

                default:
                    var defaultResult = new DefaultCommandResult(result);
                    await defaultResult.ExecuteAsync(context).ConfigureAwait(false);
                    return;
            }
        }
    }
}