using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    /// <summary>
    /// Used to send the results of command execution
    /// to in the http response.
    /// </summary>
    public interface ICommandResult
    {
        Task ExecuteAsync(HttpContext context);
    }

    public interface ICommandResult<out T> : ICommandResult
    {
        public T Result { get; }
    }
}