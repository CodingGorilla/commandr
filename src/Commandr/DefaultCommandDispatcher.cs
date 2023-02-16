using System;
using System.Reflection;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Exceptions;
using Commandr.Results;
using Commandr.Utility;
using Microsoft.AspNetCore.Http;

namespace Commandr
{
    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        private readonly Type _commandType;

        public DefaultCommandDispatcher(Type commandType)
        {
            _commandType = commandType;
        }

        public async Task Dispatch(HttpContext context)
        {
            var commandHandler = context.RequestServices.GetService(_commandType);
            if(commandHandler == null)
                throw new CommandHandlerNotFoundException(_commandType);

            var commandInvokeMethod = LocateCommandInvokeMethod();
            if(commandInvokeMethod == null)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Unable to locate the Invoke method for command: {_commandType.Name}");
                await context.Response.CompleteAsync();
                return;
            }

            var commandArguments = await new ArgumentBinder(commandInvokeMethod).GetMethodParametersAsync(context);

            var result = commandInvokeMethod.Invoke(commandHandler, commandArguments);
            object? finalResult = null;

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

            if(finalResult is ICommandResult commandResult)
                await commandResult.ExecuteAsync(context).ConfigureAwait(false);
            else
            {
                var defaultResult = new DefaultCommandResult(finalResult);
                await defaultResult.ExecuteAsync(context).ConfigureAwait(false);
            }
        }

        private MethodInfo? LocateCommandInvokeMethod()
        {
            var methods = _commandType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach(var methodInfo in methods)
            {
                var methodName = methodInfo.Name;
                if(methodName.EndsWith("Async", StringComparison.InvariantCultureIgnoreCase))
                    methodName = methodName.Remove(methodName.Length - 5, 5);

                if(methodName.EqualsIgnoreCase("handles") || methodName.EqualsIgnoreCase("invoke") || methodName.EqualsIgnoreCase("invokes") ||
                   methodName.EqualsIgnoreCase("consumes"))
                    return methodInfo;
            }

            return null;
        }
    }
}