using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromQueryStringAttribute : CommandBindingAttribute
    {
        public FromQueryStringAttribute(string parameterName)
        {
            Name = parameterName;
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.QueryParameter;
    }
}