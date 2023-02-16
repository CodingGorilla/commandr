using System;

namespace Commandr.Routing
{
    public class HttpDeleteCommandAttribute : CommandRouteAttribute
    {
        public HttpDeleteCommandAttribute(string template) : base(template, "DELETE")
        {
        }
    }
}