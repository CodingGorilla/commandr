using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class DefaultCommandResult : ICommandResult
    {
        private readonly object? _result;
        private readonly Type? _responseType;
        private readonly IResultMapper _resultMapper;

        public DefaultCommandResult(object? result, Type? responseType, IResultMapper resultMapper)
        {
            _result = result;
            _responseType = responseType;
            _resultMapper = resultMapper;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            if(_result is null)
            {
                context.Response.StatusCode = 204;
            }
            else if(_result.GetType().IsPrimitive)
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(_result.ToString() ?? string.Empty);
            }
            else
            {
                var finalResponse = _result;
                if(_responseType != null)
                    finalResponse = _resultMapper.MapResult(_result, _responseType);

                context.Response.StatusCode = 200;
                await JsonSerializer.SerializeAsync(context.Response.Body, finalResponse);
            }

            await context.Response.CompleteAsync();
        }
    }
}