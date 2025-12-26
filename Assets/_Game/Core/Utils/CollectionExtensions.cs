#nullable enable

namespace Core.Utils
{
    using System;
    using System.Collections.Generic;

    public static class CollectionExtensions
    {
        public static T DequeueOrDefault<T>(this Queue<T> queue, Func<T> valueFactory)
        {
            return queue.Count > 0 ? queue.Dequeue() : valueFactory();
        }
    }
}