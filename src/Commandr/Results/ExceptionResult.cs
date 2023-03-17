using System;
using System.Text.Json;
using System.Threading.Tasks;
using Commandr.Serialization;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    internal class ExceptionResult : ICommandResult
    {
        private readonly Exception _exception;
        private readonly ICommandSerializer _serializer;

        public ExceptionResult(Exception exception, ICommandSerializer serializer)
        {
            _exception = exception;
            _serializer = serializer;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/problem+json";
            await _serializer.SerializeResultAsync(context.Response.Body, GetProblemDetails());

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
                       Detail = _exception.StackTrace ?? String.Empty
                   };
        }

        private class ProblemDetails
        {
            public string Type { get; set; } = String.Empty;
            public string Title { get; set; } = String.Empty;
            public string Detail { get; set; } = String.Empty;
            public string Status { get; } = "500";
            public string Instance { get; set; } = String.Empty;
        }
    }
}