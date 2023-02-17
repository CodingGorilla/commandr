using System;

namespace Commandr
{
    public class DefaultCommandDispatcherFactory : ICommandDispatcherFactory
    {
        private readonly IResultMapper _mapper;

        public DefaultCommandDispatcherFactory(IResultMapper mapper)
        {
            _mapper = mapper;
        }

        public ICommandDispatcher GetDispatcher(Type commandType)
        {
            return new DefaultCommandDispatcher(commandType, _mapper);
        }
    }
}