using System;
using Commandr.Serialization;

namespace Commandr.Results
{
    public class OkContentResult : JsonContentResult
    {
        public OkContentResult(object content, ICommandSerializer serializer) : base(content, serializer)
        {
        }

        protected override int StatusCode { get; } = 200; // OK
    }
}