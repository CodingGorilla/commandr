using System;
using Commandr;
using Commandr.Binding;
using Commandr.Routing;

namespace ToDo.Commands
{
    [HttpGetCommand("/log/{msg}")]
    public class LogSomethingCommand
    {
        private readonly ILogger<LogSomethingCommand> _logger;

        public LogSomethingCommand(ILogger<LogSomethingCommand> logger)
        {
            _logger = logger;
        }

        public Task InvokeAsync([FromUrlTemplate(nameof(msg))] string msg)
        {
            this._logger.LogInformation(msg);
            return Task.CompletedTask;
        }
    }
}