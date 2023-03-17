using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Commandr.Serialization
{
    internal class DefaultCommandSerializer : ICommandSerializer
    {
        private readonly ISerializerOptionsFactory _optionsFactory;

        public DefaultCommandSerializer(ISerializerOptionsFactory optionsFactory)
        {
            _optionsFactory = optionsFactory;
        }

        public async Task SerializeResultAsync(Stream outputStream, object content)
        {
            var opts = _optionsFactory.GetSerializerOptions() as JsonSerializerOptions;
            await JsonSerializer.SerializeAsync(outputStream, content, opts);
        }

        public async Task<object?> DeserializeAsync(Stream inputStream, Type type)
        {
            var opts = _optionsFactory.GetSerializerOptions() as JsonSerializerOptions;
            return await JsonSerializer.DeserializeAsync(inputStream, type);
        }
    }
}