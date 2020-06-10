using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskLike
{
    [AsyncMethodBuilder(typeof(AsyncEnumerableTaskMethodBuilder<>))]
    public class EnumerableTask<TResult>
    {
        readonly List<TResult> _result = new List<TResult>();
        readonly EnumerableTaskAwaiter<TResult> _awaiter;

        public IEnumerable<TResult> Result => _result;

        public EnumerableTask()
        {
            _awaiter = new EnumerableTaskAwaiter<TResult>(this);
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public EnumerableTaskAwaiter<TResult> GetAwaiter()
        {
            return _awaiter;
        }

        internal void Add(TResult result)
        {
            _result.Add(result);
        }
    }
}
