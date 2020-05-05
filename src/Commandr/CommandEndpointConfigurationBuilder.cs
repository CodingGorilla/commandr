using System;
using System.Linq;
using System.Reflection;

namespace Commandr
{
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
        
        public void AddAssembly(Assembly assembly)
        {
            var commandTypes = assembly.GetTypes()
                                       .Where(t => t.GetInterfaces().Any(it => it == typeof(IRoutableCommand)))
                                       .ToList();

            commandTypes.ForEach(AddCommand);
        }

        public void AddExecutingAssembly()
        {
            AddAssembly(Assembly.GetEntryAssembly());
        }

        public void AddCommand<TCommand>() where TCommand : IRoutableCommand
        {
            AddCommand(typeof(TCommand));
        }
    }
}