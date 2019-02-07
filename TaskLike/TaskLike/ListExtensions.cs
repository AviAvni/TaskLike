using System.Collections.Generic;

namespace TaskLike
{
    static class ListExtensions
    {
        public static ListLikeAwaiter<T> GetAwaiter<T>(this IEnumerable<T> source)
        {
            return new ListLikeAwaiter<T>(source);
        }
    }
}
