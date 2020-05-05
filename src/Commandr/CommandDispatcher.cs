using System;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr
{
    public class CommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandBinder _commandBinder;

        public CommandDispatcher(Type commandType, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _commandBinder = new CommandBinder(commandType);
        }

        public async Task Dispatch(HttpContext context)
        {
            var commandBus = _serviceProvider.GetRequiredService<ICommandBus>();

            var command = await _commandBinder.GenerateCommand(context.Request);
            if(!(command is IRoutableCommand routableCommand))
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("The route resulted in a command which could not be executed");
                return;
            }

            var result = await commandBus.InvokeCommand(routableCommand);

            switch(result)
            {
                case ICommandResult commandResult:
                    await commandResult.Execute(context);
                    return;

                default:
                    var defaultResult = new DefaultCommandResult(result?.ToString() ?? "NULL");
                    await defaultResult.Execute(context);
                    return;
            }
        }
    }
}