using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


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

        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return String.Equals(value, other, StringComparison.InvariantCultureIgnoreCase);
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