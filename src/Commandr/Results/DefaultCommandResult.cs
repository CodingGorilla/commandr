﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        private static ICommandResult DetermineBestResponseType(object result)
        {
            switch(result)
            {
                case null:
                    return new StatusCodeResult(204); // No content

                case string stringResult:
                    return new PlainTextResult(200, stringResult);

                case sbyte _: 
                case byte _: 
                case short _:
                case ushort _:
                case int _: 
                case uint _: 
                case long _: 
                case ulong _:
                case float _:
                case double _: 
                case decimal _:
                    return new PlainTextResult(200, result.ToString());

                case Exception ex:
                    return new ExceptionResult(ex);

                // TODO: Others?

                default:
                    return new OkContentResult(result);
            }
        }
    }
}