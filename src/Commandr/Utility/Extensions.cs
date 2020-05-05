using System;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Buffer = System.Buffer;


namespace Commandr.Utility
{
    internal static class Extensions
    {
        public static async ValueTask<string> ReadAllText(this Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }
    }
}