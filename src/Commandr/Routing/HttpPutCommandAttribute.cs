using System;

namespace Commandr.Routing
{
    public class HttpPutCommandAttribute : CommandRouteAttribute
    {
        public HttpPutCommandAttribute(string template, Type? responseType = null)
            : base(template, "PUT", responseType)
        {
        }
    }
}