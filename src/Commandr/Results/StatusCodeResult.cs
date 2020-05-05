using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class StatusCodeResult : ICommandResult
    {
        private readonly int _statusCode;

        public StatusCodeResult(int statusCode)
        {
            _statusCode = statusCode;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = _statusCode;
            return context.Response.CompleteAsync();
        }
    }
}