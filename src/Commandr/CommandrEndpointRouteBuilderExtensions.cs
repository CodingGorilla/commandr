using System;
using Commandr.Results;
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

    public static class CommandrEndpointServiceBuilderExtensions
    {
        public static void AddCommandr(this WebApplicationBuilder builder)
        {
            AddCommandr(builder.Services);
        }

        public static void AddCommandr(this IServiceCollection serviceCollection, Action<CommandrConfigurationBuilder>? configBuilder = null)
        {
            serviceCollection.AddSingleton<ICommandResultFactory, CommandResultFactory>();
            serviceCollection.AddTransient<CommandrEndpointDataSource>();
            serviceCollection.AddTransient<ICommandDispatcherFactory, DefaultCommandDispatcherFactory>();

            if(configBuilder == null) 
                return;
            
            var builder = new CommandrConfigurationBuilder(serviceCollection);
            configBuilder.Invoke(builder);
        }
    }

    public class CommandrConfigurationBuilder
    {
        public IServiceCollection ServiceCollection { get; }

        internal CommandrConfigurationBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }
}