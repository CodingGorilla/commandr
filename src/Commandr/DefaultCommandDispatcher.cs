using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Exceptions;
using Commandr.Results;
using Commandr.Routing;
using Commandr.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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

            var commandInvokeMethod = LocateCommandInvokeMethod();
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

            if(finalResult is ICommandResult commandResult)
                await commandResult.ExecuteAsync(context).ConfigureAwait(false);
            else
            {
                var responseType = context.GetEndpoint()?.Metadata.OfType<RouteResponseTypeMetadata>().SingleOrDefault()?.RouteResponseType;
                var defaultResult = new DefaultCommandResult(finalResult, responseType, _mapper);
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