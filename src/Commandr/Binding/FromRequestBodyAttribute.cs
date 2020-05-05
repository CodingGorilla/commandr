using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FromRequestBodyAttribute : CommandBindingAttribute
    {
        public FromRequestBodyAttribute()
        {
        }

        public FromRequestBodyAttribute(string memberName)
        {
            Name = memberName;
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Body;
    }
}