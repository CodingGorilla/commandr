using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FromJsonBodyAttribute : CommandBindingAttribute
    {
        public FromJsonBodyAttribute()
        {
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Body;
    }
}