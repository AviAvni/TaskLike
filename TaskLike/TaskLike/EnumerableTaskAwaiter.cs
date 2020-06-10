using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TaskLike
{
    public class EnumerableTaskAwaiter<TResult> : ICriticalNotifyCompletion
    {
        private readonly EnumerableTask<TResult> _value;

        internal EnumerableTaskAwaiter(EnumerableTask<TResult> value) { _value = value; }

        public bool IsCompleted { get; private set; }

        public IEnumerable<TResult> GetResult()
        {
            return _value._result;
        }

        public void OnCompleted(Action continuation)
        {
            continuation();
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            continuation();
        }
    }
}
