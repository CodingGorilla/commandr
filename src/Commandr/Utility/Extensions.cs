using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
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

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(var item in items)
            {
                action.Invoke(item);
            }
        }

        public static ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter Caf<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter();
        }

        public static ConfiguredTaskAwaitable.ConfiguredTaskAwaiter Caf(this Task task)
        {
            return task.ConfigureAwait(false).GetAwaiter();
        }

        public static ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter Caf<T>(this ValueTask<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter();
        }
    }
}