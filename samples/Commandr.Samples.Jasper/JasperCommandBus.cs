using System;
using System.Threading.Tasks;
using Jasper;

namespace Commandr.Samples.Jasper
{
    public class JasperCommandBus : ICommandBus
    {
        private readonly IMessageContext _messageContext;

        public JasperCommandBus(IMessageContext messageContext)
        {
            _messageContext = messageContext;
        }

        public Task<object> InvokeCommandAsync(IRoutableCommand command)
        {
            return _messageContext.Invoke<object>(command);
        }
    }
}