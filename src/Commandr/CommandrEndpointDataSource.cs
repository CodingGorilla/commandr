using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Commandr.Metadata;
using Commandr.Routing;
using Commandr.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Commandr
{
    public class CommandrEndpointDataSource : EndpointDataSource, IEndpointConventionBuilder
    {
        private List<Endpoint>? _endpoints;
        private readonly List<Action<EndpointBuilder>> _conventions;
        private readonly List<Type> _commandTypes;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandrEndpointDataSource> _logger;

        public CommandrEndpointDataSource(IServiceProvider serviceProvider, ILogger<CommandrEndpointDataSource> logger)
        {
            _conventions = new List<Action<EndpointBuilder>>();
            _commandTypes = new List<Type>();

            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void AddCommandType(Type commandType)
        {
            _commandTypes.Add(commandType);
        }

        public override IChangeToken GetChangeToken()
        {
            return NullChangeToken.Singleton;
        }

        public override IReadOnlyList<Endpoint> Endpoints => _endpoints ??= BuildEndpoints();

        public void Add(Action<EndpointBuilder> convention)
        {
            _conventions.Add(convention);
        }

        private List<Endpoint> BuildEndpoints()
        {
            var endpoints = new List<Endpoint>();

            if(!_commandTypes.Any())
                return endpoints;

            var dispatcherFactory = _serviceProvider.GetRequiredService<ICommandDispatcherFactory>();

            var endpointBuilder = new CommandEndpointBuilder(_logger, dispatcherFactory, _conventions);
            foreach(var commandType in _commandTypes)
            {
                var ep = endpointBuilder.BuildEndpointForCommandType(commandType);
                if(ep != null) 
                    endpoints.Add(ep);
            }

            return endpoints;
        }

        private class CommandEndpointBuilder
        {
            private readonly ILogger<CommandrEndpointDataSource> _logger;
            private readonly ICommandDispatcherFactory _dispatcherFactory;
            private readonly List<Action<EndpointBuilder>> _conventions;

            public CommandEndpointBuilder(ILogger<CommandrEndpointDataSource> logger, ICommandDispatcherFactory dispatcherFactory,
                                          List<Action<EndpointBuilder>> conventions)
            {
                _logger = logger;
                _dispatcherFactory = dispatcherFactory;
                _conventions = conventions;
            }

            public Endpoint? BuildEndpointForCommandType(Type cmdType)
            {
                var routeAttribute = cmdType.GetCustomAttribute<CommandRouteAttribute>();
                if(routeAttribute == null)
                {
                    _logger.LogTrace("No routing attributes found for command: {CmdTypeName}", cmdType.Name);
                    return null;
                }

                var invokeMethodInfo = LocateCommandInvokeMethod(cmdType);
                if(invokeMethodInfo == null)
                {
                    _logger.LogTrace("No invoke method found for command: {CmdTypeName}", cmdType.Name);
                    return null;
                }

                var authorizeAttribute = cmdType.GetCustomAttribute<AuthorizeAttribute>();

                var commandDispatcher = _dispatcherFactory.GetDispatcher(cmdType);

                var endpointBuilder = new RouteEndpointBuilder(
                                          commandDispatcher.Dispatch,
                                          RoutePatternFactory.Parse(routeAttribute.Template),
                                          routeAttribute.Order)
                                      {
                                          DisplayName = $"Command: {cmdType.Name}",
                                      };

                endpointBuilder.Metadata.Add(new HttpMethodMetadata(new[] { routeAttribute.Method }));
                endpointBuilder.Metadata.Add(invokeMethodInfo);

                if(routeAttribute.ResponseType != null)
                    endpointBuilder.Metadata.Add(new RouteResponseTypeMetadata(routeAttribute.ResponseType));

                if(authorizeAttribute != null)
                    endpointBuilder.Metadata.Add(authorizeAttribute);

                foreach(var convention in _conventions)
                {
                    convention(endpointBuilder);
                }

                return endpointBuilder.Build();
            }

            private static MethodInfo? LocateCommandInvokeMethod(Type commandType)
            {
                var methods = commandType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach(var methodInfo in methods)
                {
                    var methodName = methodInfo.Name;
                    if(methodName.EndsWith("Async", StringComparison.InvariantCultureIgnoreCase))
                        methodName = methodName.Remove(methodName.Length - 5, 5);

                    if(methodName.EqualsIgnoreCase("handles") || methodName.EqualsIgnoreCase("invoke") ||
                       methodName.EqualsIgnoreCase("invokes") || methodName.EqualsIgnoreCase("consumes"))
                        return methodInfo;
                }

                return null;
            }
        }
    }
}