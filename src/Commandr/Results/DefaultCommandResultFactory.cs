using System;

namespace Commandr.Results
{
    public interface ICommandResultFactory
    {
        ICommandResult Status(int statusCode);
        ICommandResult Status(int statusCode, object content);
        ICommandResult NotFound();
        ICommandResult Unauthorized();
        ICommandResult Forbidden();
        ICommandResult Ok();
        ICommandResult Ok(object content);
        ICommandResult Created(string location, object? instance);
        ICommandResult Accepted();
        ICommandResult Accepted(string location);
    }

    internal class DefaultCommandResultFactory : ICommandResultFactory
    {
        public ICommandResult Status(int statusCode)
            => new StatusCodeResult(statusCode);

        public ICommandResult Status(int statusCode, object content)
            => new ContentStatusCodeResult(content, statusCode);

        public ICommandResult NotFound()
            => new NotFoundResult();

        public ICommandResult Unauthorized()
            => new StatusCodeResult(401);

        public ICommandResult Forbidden()
            => new StatusCodeResult(403);

        public ICommandResult Ok()
            => new StatusCodeResult(200);

        public ICommandResult Ok(object content)
            => new OkContentResult(content);

        public ICommandResult Created(string location, object? instance)
            => new LocationResult(201, location, instance);

        public ICommandResult Accepted()
            => new StatusCodeResult(202);

        public ICommandResult Accepted(string location)
            => new LocationResult(202, location, null);
    }
}