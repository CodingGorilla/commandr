using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Metadata;

namespace Commandr.Metadata
{
    public class RouteResponseTypeMetadata : IProducesResponseTypeMetadata
    {
        public RouteResponseTypeMetadata(Type routeResponseType)
        {
            Type = routeResponseType;
        }

        public Type? Type { get; }
        public int StatusCode { get; } = 200;
        public IEnumerable<string> ContentTypes { get; } = new[] { "application/json" };
    }
}