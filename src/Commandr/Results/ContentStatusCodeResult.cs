using System;

namespace Commandr.Results
{
    public class ContentStatusCodeResult : ContentResult
    {
        public ContentStatusCodeResult(object content, int statusCode) : base(content)
        {
            StatusCode = statusCode;
        }

        protected override int StatusCode { get; }
    }
}