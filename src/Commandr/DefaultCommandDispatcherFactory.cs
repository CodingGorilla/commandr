using System;

namespace Commandr
{
    public class DefaultCommandDispatcherFactory : ICommandDispatcherFactory
    {
        public ICommandDispatcher GetDispatcher(Type commandType)
        {
            return new DefaultCommandDispatcher(commandType);
        }
    }
}