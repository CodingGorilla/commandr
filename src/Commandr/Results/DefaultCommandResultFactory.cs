using System;
using Commandr.Serialization;

namespace Commandr.Results
{
    public interface ICommandResultFactory
    {
        ICommandResult Status(int statusCode);
        ICommandResult<T> Status<T>(int statusCode, T content);
        ICommandResult NotFound();
        ICommandResult Unauthorized();
        ICommandResult Forbidden();
        ICommandResult Ok();
        ICommandResult<T> Ok<T>(T content);
        ICommandResult Created(string location, object? instance);
        ICommandResult Accepted();
        ICommandResult Accepted(string location);
    }

    internal class DefaultCommandResultFactory : ICommandResultFactory
    {
        private readonly ICommandSerializer _serializer;

        public DefaultCommandResultFactory(ICommandSerializer serializer)
        {
            _serializer = serializer;
        }

        public ICommandResult Status(int statusCode)
            => new StatusCodeResult(statusCode);

        public ICommandResult<T> Status<T>(int statusCode, T content)
            => new ObjectContentResult<T>(content, statusCode, _serializer);

        public ICommandResult NotFound()
            => new NotFoundResult();

        public ICommandResult Unauthorized()
            => new StatusCodeResult(401);

        public ICommandResult Forbidden()
            => new StatusCodeResult(403);

        public ICommandResult Ok()
            => new StatusCodeResult(200);

        public ICommandResult<T> Ok<T>(T content)
            => new ObjectContentResult<T>(content, _serializer);

        public ICommandResult Created(string location, object? instance)
            => new LocationResult(201, location, instance, _serializer);

        public ICommandResult Accepted()
            => new StatusCodeResult(202);

        public ICommandResult Accepted(string location)
            => new LocationResult(202, location, null, _serializer);
    }
}