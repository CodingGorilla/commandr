using System;
using System.Text.Json;
using Commandr.Results;
using Commandr.Serialization;
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

        public static void AddCommandr(this WebApplicationBuilder builder, Action<CommandrConfigurationBuilder>? configBuilder)
        {
            AddCommandr(builder.Services, configBuilder);
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

        public Action<IServiceCollection> RegisterResultMapper { get; set; }

        public Action<IServiceCollection> RegisterResultFactory { get; set; }

        public Action<IServiceCollection> RegisterResultSerializer { get; set; }

        public Action<IServiceCollection> RegisterSerializerOptionsFactory { get; set; }

        private Func<DefaultSerializerOptionsFactory> _serializerOptionsFactoryFactory;

        public void AddJsonSerializerOptions(JsonSerializerOptions options)
        {
            _serializerOptionsFactoryFactory = () => new DefaultSerializerOptionsFactory(options);
        }

        internal CommandrConfigurationBuilder(IServiceCollection serviceCollection)
        {
            _serializerOptionsFactoryFactory = () => new DefaultSerializerOptionsFactory();

            ServiceCollection = serviceCollection;
            
            RegisterCommandDispatcher = sc => sc.AddTransient<ICommandDispatcherFactory, DefaultCommandDispatcherFactory>();
            RegisterResultMapper = sc => sc.AddTransient<IResultMapper, DefaultResultMapper>();
            RegisterResultFactory = sc => sc.AddSingleton<ICommandResultFactory, DefaultCommandResultFactory>();
            RegisterResultSerializer = sc => sc.AddTransient<ICommandSerializer, DefaultCommandSerializer>();
            RegisterSerializerOptionsFactory = sc => sc.AddSingleton<ISerializerOptionsFactory>(_serializerOptionsFactoryFactory.Invoke());
        }

        internal void RegisterDependencies()
        {
            RegisterCommandDispatcher(ServiceCollection);
            RegisterResultMapper(ServiceCollection);
            RegisterResultFactory(ServiceCollection);
            RegisterResultSerializer(ServiceCollection);
            RegisterSerializerOptionsFactory(ServiceCollection);
        }
    }
}