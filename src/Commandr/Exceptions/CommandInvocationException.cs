using System;

namespace Commandr.Exceptions
{
    public class CommandInvocationException : Exception
    {
        public CommandInvocationException(Type commandType, string methodName, Exception innerException)
            : base($"An exception occurred while invoking the method: {methodName} on the command type: {commandType}", innerException)
        {
            throw new NotImplementedException();
        }
    }
}