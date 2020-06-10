using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TaskLike
{
    public class EnumerableAwaiter<TResult> : ICriticalNotifyCompletion
    {
        private readonly IEnumerable<TResult> _source;
        private IEnumerator<TResult> _value;

        internal EnumerableAwaiter(IEnumerable<TResult> value)
        {
            _source = value;
            _value = _source.GetEnumerator();
        }

        public bool IsCompleted { get; private set; }

        public TResult GetResult()
        {
            return _value.Current;
        }

        public bool MoveNext()
        {
            IsCompleted = !_value.MoveNext();
            return IsCompleted;
        }

        public void Reset()
        {
            _value = _source.GetEnumerator();
            IsCompleted = false;
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
