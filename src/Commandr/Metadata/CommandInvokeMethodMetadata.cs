using System;
using System.Reflection;

namespace Commandr.Metadata
{
    public class CommandInvokeMethodMetadata
    {
        public CommandInvokeMethodMetadata(MethodInfo commandInvokeMethod)
        {
            CommandInvokeMethod = commandInvokeMethod;
        }

        public MethodInfo CommandInvokeMethod { get; }
    }
}