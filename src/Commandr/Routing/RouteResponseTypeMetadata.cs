using System;

namespace Commandr.Routing
{
    public class RouteResponseTypeMetadata
    {
        public RouteResponseTypeMetadata(Type routeResponseType)
        {
            RouteResponseType = routeResponseType;
        }

        public Type RouteResponseType { get; }
    }
}