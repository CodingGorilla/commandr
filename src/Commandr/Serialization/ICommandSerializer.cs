using System;
using System.IO;
using System.Threading.Tasks;

namespace Commandr.Serialization
{
    public interface ICommandSerializer
    {
        Task SerializeResultAsync(Stream outputStream, object content);
        Task<object?> DeserializeAsync(Stream inputStream, Type type);
    }
}