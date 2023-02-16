using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    internal class ExceptionResult : ICommandResult
    {
        private readonly Exception _exception;

        public ExceptionResult(Exception exception)
        {
            _exception = exception;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/problem+json";
            await JsonSerializer.SerializeAsync(context.Response.Body, GetProblemDetails());

            await context.Response.CompleteAsync();
        }

        /// <summary>
        /// Generates an instance that can be serialized to an
        /// RFC 7807 Problem Details response
        /// </summary>
        /// <returns></returns>
        private ProblemDetails GetProblemDetails()
        {
            return new ProblemDetails
                   {
                       Title = _exception.Message,
                       Detail = _exception.StackTrace
                   };
        }

        private class ProblemDetails
        {
            public string Type { get; set; }
            public string Title { get; set; }
            public string Detail { get; set; }
            public string Status { get; } = "500";
            public string Instance { get; set; }
        }
    }
}