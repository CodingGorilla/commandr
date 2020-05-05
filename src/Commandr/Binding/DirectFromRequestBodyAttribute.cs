using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DirectFromRequestBodyAttribute : CommandBindingAttribute
    {
        public DirectFromRequestBodyAttribute()
        {
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Body;
    }
}