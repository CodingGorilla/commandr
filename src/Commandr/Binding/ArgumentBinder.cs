using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Commandr.Serialization;
using Microsoft.AspNetCore.Http;

namespace Commandr.Binding
{
    internal class ArgumentBinder
    {
        private readonly MethodInfo _handlerMethod;
        private readonly ICommandSerializer _serializer;

        public ArgumentBinder(MethodInfo handlerMethod, ICommandSerializer serializer)
        {
            _handlerMethod = handlerMethod;
            _serializer = serializer;
        }

        public async Task<object?[]> GetMethodParametersAsync(HttpContext context)
        {
            var parameters = _handlerMethod.GetParameters();

            var methodParameters = new object?[parameters.Length];
            for(var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if(parameter.ParameterType == typeof(CancellationToken))
                {
                    methodParameters[i] = CancellationToken.None; // TODO: Send real cancellation
                    continue;
                }

                methodParameters[i] = await GetParameterFromRequestAsync(parameter, context.Request);
            }

            return methodParameters;
        }

        private async Task<object?> GetParameterFromRequestAsync(ParameterInfo parameter, HttpRequest request)
        {
            var bindingAttribute = parameter.GetCustomAttribute<CommandBindingAttribute>();
            switch(bindingAttribute?.Location)
            {
                case RequestBindingLocation.Body:
                    return await _serializer.DeserializeAsync(request.Body, parameter.ParameterType);
                case RequestBindingLocation.Url:
                    return request.RouteValues.TryGetValue(bindingAttribute.Name, out var urlResult) ? urlResult : null;
                case RequestBindingLocation.QueryParameter:
                    return request.Query.TryGetValue(bindingAttribute.Name, out var queryResult) ? queryResult.SingleOrDefault() : null;
                case RequestBindingLocation.FormField:
                    return request.Form.TryGetValue(bindingAttribute.Name, out var formResult) ? formResult.SingleOrDefault() : null;
                default:
                    return null;
            }
        }
    }
}