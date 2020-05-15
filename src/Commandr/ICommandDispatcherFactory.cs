using System;

namespace Commandr
{
    public interface ICommandDispatcherFactory
    {
        ICommandDispatcher GetDispatcher(Type commandType);
    }
}