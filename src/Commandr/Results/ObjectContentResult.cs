using System;
using Commandr.Serialization;

namespace Commandr.Results
{
    public class ObjectContentResult<T> : JsonContentResult, ICommandResult<T>
    {
        public ObjectContentResult(T result, ICommandSerializer serializer) : this(result, 200, serializer)
        {
        }

        public ObjectContentResult(T result, int statusCode, ICommandSerializer serializer) : base(result!, serializer)
        {
            StatusCode = statusCode;
        }

        protected override int StatusCode { get; }

        public T Result => (T)Content;
    }
}