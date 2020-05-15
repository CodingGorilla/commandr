using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class DefaultCommandResult : ICommandResult
    {
        private readonly object _result;

        public DefaultCommandResult(object result)
        {
            _result = result;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            if(!(_result is null))
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(_result.ToString());
            }
            else
            {
                context.Response.StatusCode = 204;
            }

            await context.Response.CompleteAsync();
        }
    }
}