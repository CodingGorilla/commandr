using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FromRequestUrl : CommandBindingAttribute
    {
        public FromRequestUrl(string parameterName)
        {
            Name = parameterName;
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Url;
    }
}