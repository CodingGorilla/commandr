using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class NotFoundResult : ICommandResult
    {
        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 404;
            return context.Response.CompleteAsync();
        }
    }
}