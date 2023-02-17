using System;

namespace Commandr.Exceptions
{
    public class CommandHandlerInvokeMethodNotFoundException : Exception
    {
        public CommandHandlerInvokeMethodNotFoundException(Type commandType)
            : base($"Unable to locate the invocation method for the command type: {commandType}")
        {
        }
    }
}