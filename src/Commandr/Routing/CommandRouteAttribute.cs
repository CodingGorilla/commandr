using System;

namespace Commandr.Routing
{
    /// <summary>
    /// Defines routing information for a command
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandRouteAttribute : Attribute
    {
        public CommandRouteAttribute(string template, string method)
        {
            Template = template;
            Method = method;
        }

        /// <summary>
        /// The route template use by endpoint routing to execute this command
        /// </summary>
        public string Template { get; }

        /// <summary>
        /// The HTTP verb used to execute this command
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// The endpoint routing evaluation order
        /// </summary>
        public int Order { get; set; }
    }
}