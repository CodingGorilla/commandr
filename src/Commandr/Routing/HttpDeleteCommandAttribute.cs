using System;

namespace Commandr.Routing
{
    public class HttpDeleteCommandAttribute : CommandRouteAttribute
    {
        public HttpDeleteCommandAttribute(string template, Type? responseType = null) 
            : base(template, "DELETE", responseType)
        {
        }
    }
}