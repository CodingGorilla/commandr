namespace Commandr.Routing
{
    public class HttpPutCommandAttribute : CommandRouteAttribute
    {
        public HttpPutCommandAttribute(string template) : base(template, "PUT")
        {
        }
    }
}