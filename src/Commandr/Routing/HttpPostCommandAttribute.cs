using System;

namespace Commandr.Routing
{
    public class HttpPostCommandAttribute : CommandRouteAttribute
    {
        public HttpPostCommandAttribute(string template, Type? responseType = null)
            : base(template, "POST", responseType)
        {
        }
    }
}