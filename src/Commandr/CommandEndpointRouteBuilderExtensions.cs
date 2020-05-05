using System;
using Commandr.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr
{
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

    public static class CommandEndpointServiceBuilderExtensions
    {
        public static void AddCommandr(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICommandResultFactory, CommandResultFactory>();
        }
    }
}