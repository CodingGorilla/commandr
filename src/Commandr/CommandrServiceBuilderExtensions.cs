using System;
using Commandr.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr
{
    public static class CommandrServiceBuilderExtensions
    {
        public static void AddCommandr(this WebApplicationBuilder builder)
        {
            AddCommandr(builder.Services);
        }

        public static void AddCommandr(this IServiceCollection serviceCollection, Action<CommandrConfigurationBuilder>? configBuilder = null)
        {
            serviceCollection.AddTransient<CommandrEndpointDataSource>();

            var builder = new CommandrConfigurationBuilder(serviceCollection);
            configBuilder?.Invoke(builder);

            builder.RegisterDependencies();
        }
    }

    public class CommandrConfigurationBuilder
    {
        public IServiceCollection ServiceCollection { get; }

        public Action<IServiceCollection> RegisterCommandDispatcher { get; set; }
            = sc => sc.AddTransient<ICommandDispatcherFactory, DefaultCommandDispatcherFactory>();

        public Action<IServiceCollection> RegisterResultMapper { get; set; }
            = sc => sc.AddTransient<IResultMapper, DefaultResultMapper>();

        public Action<IServiceCollection> RegisterResultFactory { get; set; } =
            sc => sc.AddSingleton<ICommandResultFactory, DefaultCommandResultFactory>();

        internal CommandrConfigurationBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        internal void RegisterDependencies()
        {
            RegisterCommandDispatcher(ServiceCollection);
            RegisterResultMapper(ServiceCollection);
            RegisterResultFactory(ServiceCollection);
        }
    }
}