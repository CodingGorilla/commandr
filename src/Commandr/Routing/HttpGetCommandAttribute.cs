namespace Commandr.Routing
{
    public class HttpGetCommandAttribute : CommandRouteAttribute
    {
        public HttpGetCommandAttribute(string template) : base(template, "GET")
        {
        }
    }
}