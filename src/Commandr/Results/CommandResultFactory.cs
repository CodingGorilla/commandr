using System;

namespace Commandr.Results
{
    public interface ICommandResultFactory
    {
        ICommandResult Status(int statusCode);
        ICommandResult Status(int statusCode, object content);
        ICommandResult NotFound();
        ICommandResult Ok();
        ICommandResult Ok(object content);
    }

    internal class CommandResultFactory : ICommandResultFactory
    {
        public ICommandResult Status(int statusCode) 
            => new StatusCodeResult(statusCode);

        public ICommandResult Status(int statusCode, object content) 
            => new ContentStatusCodeResult(content, statusCode);

        public ICommandResult NotFound() 
            => new NotFoundResult();

        public ICommandResult Ok() 
            => new StatusCodeResult(200);

        public ICommandResult Ok(object content) 
            => new OkContentResult(content);
    }
}