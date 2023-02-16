using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Commandr.Routing;
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

            // Step 1: Locate all of the commands
            foreach(var cmdType in _commandTypes)
            {
                var routeAttribute = cmdType.GetCustomAttribute<CommandRouteAttribute>();
                if(routeAttribute == null)
                {
                    _logger.LogTrace("No routing attributes found for command: {CmdTypeName}", cmdType.Name);
                    continue;
                }

                var authorizeAttribute = cmdType.GetCustomAttribute<AuthorizeAttribute>();

                var commandDispatcher = dispatcherFactory.GetDispatcher(cmdType);

                var endpointBuilder = new RouteEndpointBuilder(
                                          context => commandDispatcher.Dispatch(context),
                                          RoutePatternFactory.Parse(routeAttribute.Template),
                                          routeAttribute.Order)
                                      {
                                          DisplayName = $"Command: {cmdType.Name}"
                                      };
                
                endpointBuilder.Metadata.Add(new HttpMethodMetadata(new[] { routeAttribute.Method }));

                if(authorizeAttribute != null)
                    endpointBuilder.Metadata.Add(authorizeAttribute);

                foreach(var convention in _conventions)
                {
                    convention(endpointBuilder);
                }

                endpoints.Add(endpointBuilder.Build());
            }

            return endpoints;
        }
    }
}