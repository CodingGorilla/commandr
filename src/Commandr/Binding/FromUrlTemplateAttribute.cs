using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromUrlTemplateAttribute : CommandBindingAttribute
    {
        public FromUrlTemplateAttribute(string parameterName)
        {
            Name = parameterName;
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.Url;
    }
}