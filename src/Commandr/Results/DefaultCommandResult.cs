using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr.Results
{
    public class DefaultCommandResult : ICommandResult
    {
        private readonly string _result;

        public DefaultCommandResult(string result)
        {
            _result = result;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}