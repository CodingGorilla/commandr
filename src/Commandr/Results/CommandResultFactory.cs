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
        {
            return new StatusCodeResult(statusCode);
        }

        public ICommandResult Status(int statusCode, object content)
        {
            return new ContentStatusCodeResult(content, statusCode);
        }

        public ICommandResult NotFound()
        {
            return new NotFoundResult();
        }

        public ICommandResult Ok()
        {
            return new StatusCodeResult(200);
        }

        public ICommandResult Ok(object content)
        {
            return new OkContentResult(content);
        }
    }
}