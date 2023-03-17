using System;

namespace Commandr.Serialization
{
    public interface ISerializerOptionsFactory
    {
        void SetSerializerOptions(object serializerOptions);
        object? GetSerializerOptions();
    }
}