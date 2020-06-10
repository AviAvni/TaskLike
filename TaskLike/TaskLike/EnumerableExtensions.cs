using System.Collections.Generic;

namespace TaskLike
{
    static class EnumerableExtensions
    {
        public static EnumerableAwaiter<T> GetAwaiter<T>(this IEnumerable<T> source)
        {
            return new EnumerableAwaiter<T>(source);
        }
    }
}
