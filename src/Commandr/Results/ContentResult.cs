using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
            var contentString = JsonConvert.SerializeObject(_content);
            await context.Response.WriteAsync(contentString);

            await context.Response.CompleteAsync();
        }
    }
}