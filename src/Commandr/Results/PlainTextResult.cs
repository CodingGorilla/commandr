using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    internal class PlainTextResult : ICommandResult
    {
        private readonly int _statusCode;
        private readonly string _result;

        public PlainTextResult(int statusCode, string result)
        {
            _statusCode = statusCode;
            _result = result;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = _statusCode;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(_result);

            await context.Response.CompleteAsync();
        }
    }
}