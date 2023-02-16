using System;

namespace Commandr.Exceptions
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException(Type commandType)
            : base($"Unable to locate a command handler for: {commandType.Name}")
        {
        }
    }
}