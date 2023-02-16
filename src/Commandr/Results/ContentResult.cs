using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public abstract class ContentResult : ICommandResult
    {
        private readonly object _content;

        protected ContentResult(object content)
        {
            _content = content;
        }

        protected abstract int StatusCode { get; }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = StatusCode;

            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(context.Response.Body, _content);

            await context.Response.CompleteAsync();
        }
    }
}