using System;
using System.Text.Json;
using System.Threading.Tasks;
using Commandr.Serialization;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public abstract class JsonContentResult : ICommandResult
    {
        private readonly ICommandSerializer _serializer;

        protected JsonContentResult(object content, ICommandSerializer serializer)
        {
            _serializer = serializer;
            Content = content;
        }
        
        protected abstract int StatusCode { get; }
        protected object Content { get; }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCode;

            context.Response.ContentType = "application/json";
            await _serializer.SerializeResultAsync(context.Response.Body, Content);

            await context.Response.CompleteAsync();
        }
    }
}