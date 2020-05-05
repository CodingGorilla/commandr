using System;
using System.Collections.Generic;
using System.Reflection;
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
    public class CommandEndpointDataSource : EndpointDataSource, IEndpointConventionBuilder
    {
        private List<Endpoint> _endpoints;
        private readonly List<Action<EndpointBuilder>> _conventions;
        private readonly List<Type> _commandTypes;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandEndpointDataSource> _logger;

        public CommandEndpointDataSource(IServiceProvider serviceProvider, ILogger<CommandEndpointDataSource> logger)
        {
            _conventions = new List<Action<EndpointBuilder>>();
            _commandTypes = new List<Type>();

            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void AddCommandType(Type commandType)
        {
            if(commandType.IsAssignableFrom(typeof(IRoutableCommand)))
                throw new ArgumentException("The specified type is not an IRoutableCommand");

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

            // Step 1: Locate all of the commands
            foreach(var cmdType in _commandTypes)
            {
                var routeAttribute = cmdType.GetCustomAttribute<CommandRouteAttribute>();
                if(routeAttribute == null)
                {
                    _logger.LogTrace($"No routing attributes found for command: {cmdType.Name}");
                    continue;
                }

                var authorizeAttribute = cmdType.GetCustomAttribute<AuthorizeAttribute>();

                var commandDispatcher = new CommandDispatcher(cmdType, _serviceProvider);

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

    public class CommandEndpointConfigurationBuilder
    {
        private readonly CommandEndpointDataSource _dataSource;

        public CommandEndpointConfigurationBuilder(CommandEndpointDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void AddCommand(Type commandType)
        {
            _dataSource.AddCommandType(commandType);
        }

        public void AddCommand<TCommand>() where TCommand : IRoutableCommand
        {
            AddCommand(typeof(TCommand));
        }
    }

    public static class CommandEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapCommands(this IEndpointRouteBuilder endpoints, Action<CommandEndpointConfigurationBuilder> configure)
        {
            var dataSource = endpoints.ServiceProvider.GetRequiredService<CommandEndpointDataSource>();

            var configBuilder = new CommandEndpointConfigurationBuilder(dataSource);
            configure(configBuilder);

            endpoints.DataSources.Add(dataSource);
            return dataSource;
        }
    }
}