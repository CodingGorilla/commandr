using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FromJsonTokenAttribute : CommandBindingAttribute
    {
        public FromJsonTokenAttribute()
        {
        }

        public FromJsonTokenAttribute(string memberName)
        {
            Name = memberName;
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Body;
    }
}