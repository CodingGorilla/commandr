using System;
using System.Text.Json;
using System.Threading.Tasks;
using Commandr.Serialization;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class DefaultCommandResult : ICommandResult
    {
        private readonly object? _result;
        private readonly Type? _responseType;
        private readonly IResultMapper _resultMapper;
        private readonly ICommandSerializer _serializer;

        public DefaultCommandResult(object? result, Type? responseType,
                                    IResultMapper resultMapper, ICommandSerializer serializer)
        {
            _result = result;
            _responseType = responseType;
            _resultMapper = resultMapper;
            _serializer = serializer;
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
                context.Response.Headers.ContentType = "text/plain";
                await context.Response.WriteAsync(_result.ToString() ?? string.Empty);
            }
            else if(_result.GetType().IsAssignableTo(_responseType))
            {
                context.Response.StatusCode = 200;
                context.Response.Headers.ContentType = "application/json";
                await _serializer.SerializeResultAsync(context.Response.Body, _result);
            }
            else
            {
                var finalResponse = _result;
                if(_responseType != null)
                    finalResponse = _resultMapper.MapResult(_result, _responseType);

                context.Response.StatusCode = 200;
                context.Response.Headers.ContentType = "application/json";
                await _serializer.SerializeResultAsync(context.Response.Body, finalResponse);
            }

            await context.Response.CompleteAsync();
        }
    }
}