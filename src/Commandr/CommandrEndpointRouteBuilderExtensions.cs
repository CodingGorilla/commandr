using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr
{
    public static class CommandrEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapCommands(this IEndpointRouteBuilder endpoints)
            => MapCommands(endpoints, _ => _.AddExecutingAssembly());

        public static IEndpointConventionBuilder MapCommands(this IEndpointRouteBuilder endpoints, Action<CommandEndpointConfigurationBuilder> configure)
        {
            var dataSource = endpoints.ServiceProvider.GetRequiredService<CommandrEndpointDataSource>();

            var configBuilder = new CommandEndpointConfigurationBuilder(dataSource);
            configure(configBuilder);

            endpoints.DataSources.Add(dataSource);
            return dataSource;
        }
    }
    }