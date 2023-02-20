using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class LocationResult : ICommandResult
    {
        private readonly int _statusCode;

        public LocationResult(int statusCode, string location, object? instance)
        {
            _statusCode = statusCode;
            Instance = instance;
            Location = location;
        }

        public object? Instance { get; }
        public string Location { get; }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = _statusCode;
            context.Response.Headers.Location = Location;

            if(Instance != null)
            {
                context.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(context.Response.Body, Instance);
            }
        }
    }
}