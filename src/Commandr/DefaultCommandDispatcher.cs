using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Exceptions;
using Commandr.Metadata;
using Commandr.Results;
using Microsoft.AspNetCore.Http;

namespace Commandr
{
    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        private readonly Type _commandType;
        private readonly IResultMapper _mapper;

        public DefaultCommandDispatcher(Type commandType, IResultMapper mapper)
        {
            _commandType = commandType;
            _mapper = mapper;
        }

        public async Task Dispatch(HttpContext context)
        {
            var commandHandler = context.RequestServices.GetService(_commandType);
            if(commandHandler == null)
                throw new CommandHandlerNotFoundException(_commandType);

            var commandInvokeMethod = GetCommandInvokeMethod(context);
            if(commandInvokeMethod == null)
                throw new CommandHandlerInvokeMethodNotFoundException(_commandType);

            var commandArguments = await new ArgumentBinder(commandInvokeMethod).GetMethodParametersAsync(context);

            object? result;
            object? finalResult = null;

            try
            {
                result = commandInvokeMethod.Invoke(commandHandler, commandArguments);
            }
            catch(Exception ex)
            {
                throw new CommandInvocationException(_commandType, commandInvokeMethod.Name, ex);
            }

            var resultType = result?.GetType() ?? typeof(void);
            if(resultType.BaseType == typeof(Task) || resultType.BaseType?.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var task = (Task)result!;
                await task;

                if(resultType.IsGenericType)
                    finalResult = task.GetType().GetProperty("Result")?.GetValue(task);
            }
            else
            {
                finalResult = result;
            }

            await HandleCommandResult(context, finalResult);
        }

        private async Task HandleCommandResult(HttpContext context, object? finalResult)
        {
            if(finalResult is ICommandResult commandResult)
                await commandResult.ExecuteAsync(context).ConfigureAwait(false);
            else
            {
                var responseType = context.GetEndpoint()?.Metadata.OfType<RouteResponseTypeMetadata>().SingleOrDefault()?.Type;
                var defaultResult = new DefaultCommandResult(finalResult, responseType, _mapper);
                await defaultResult.ExecuteAsync(context).ConfigureAwait(false);
            }
        }

        private static MethodInfo GetCommandInvokeMethod(HttpContext context)
            => context.GetEndpoint()!.Metadata.OfType<MethodInfo>().Single();
    }
}