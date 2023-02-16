using System;

namespace Commandr.Binding
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromFormFieldAttribute : CommandBindingAttribute
    {
        public FromFormFieldAttribute(string formFieldName)
        {
            Name = formFieldName;
        }

        internal override RequestBindingLocation Location { get; } = RequestBindingLocation.FormField;
    }
}