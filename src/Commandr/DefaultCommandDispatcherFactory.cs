using System;
using Commandr.Serialization;

namespace Commandr
{
    public class DefaultCommandDispatcherFactory : ICommandDispatcherFactory
    {
        private readonly IResultMapper _mapper;
        private readonly ICommandSerializer _serializer;

        public DefaultCommandDispatcherFactory(IResultMapper mapper, ICommandSerializer serializer)
        {
            _mapper = mapper;
            _serializer = serializer;
        }

        public ICommandDispatcher GetDispatcher(Type commandType)
        {
            return new DefaultCommandDispatcher(commandType, _mapper, _serializer);
        }
    }
}