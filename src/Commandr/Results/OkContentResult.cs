using System;
using Microsoft.Extensions.DependencyInjection;

namespace Commandr.Results
{
    public class OkContentResult : ContentResult
    {
        public OkContentResult(object content) : base(content)
        {
        }

        protected override int StatusCode { get; } = 200; // OK
    }
}