using System;
using System.Linq;
using System.Reflection;
using Commandr.Routing;

namespace Commandr
{
    public class CommandEndpointConfigurationBuilder
    {
        private readonly CommandrEndpointDataSource _dataSource;

        public CommandEndpointConfigurationBuilder(CommandrEndpointDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void AddCommand(Type commandType)
        {
            _dataSource.AddCommandType(commandType);
        }
        
        public void AddAssembly(Assembly assembly)
        {
            var commandTypes = assembly.GetTypes()
                                       .Where(t => t.GetCustomAttributes<CommandRouteAttribute>().Any())
                                       .ToList();

            commandTypes.ForEach(AddCommand);
        }

        public void AddExecutingAssembly()
        {
            AddAssembly(Assembly.GetEntryAssembly());
        }

        public void AddCommand<TCommand>()
        {
            AddCommand(typeof(TCommand));
        }
    }
}