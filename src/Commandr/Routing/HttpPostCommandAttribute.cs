namespace Commandr.Routing
{
    public class HttpPostCommandAttribute : CommandRouteAttribute
    {
        public HttpPostCommandAttribute(string template) : base(template, "POST")
        {
        }
    }
}