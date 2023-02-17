using System;

namespace Commandr.Routing
{
    public class HttpGetCommandAttribute : CommandRouteAttribute
    {
        public HttpGetCommandAttribute(string template, Type? responseType = null) 
            : base(template, "GET", responseType)
        {
        }
    }
}