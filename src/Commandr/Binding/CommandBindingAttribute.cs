using System;

namespace Commandr.Binding
{
    public abstract class CommandBindingAttribute : Attribute
    {
        internal abstract RequestBindingLocation Location { get; }
        public string Name { get; protected init; } = String.Empty;
    }
}