using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromJsonBodyAttribute : CommandBindingAttribute
    {
        public FromJsonBodyAttribute()
        {
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Body;
    }
}