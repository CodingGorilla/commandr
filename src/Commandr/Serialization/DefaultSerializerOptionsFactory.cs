using System;
using System.Text.Json;

namespace Commandr.Serialization
{
    internal class DefaultSerializerOptionsFactory : ISerializerOptionsFactory
    {
        private object? _serializerOptions;

        public DefaultSerializerOptionsFactory()
        {
        }

        public DefaultSerializerOptionsFactory(JsonSerializerOptions options)
            => _serializerOptions = options;

        public void SetSerializerOptions(object serializerOptions)
            => _serializerOptions = serializerOptions;

        public object? GetSerializerOptions()
            => _serializerOptions;
    }
}