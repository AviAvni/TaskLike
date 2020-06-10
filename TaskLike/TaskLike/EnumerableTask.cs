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
        internal readonly List<TResult> _result;
        EnumerableTaskAwaiter<TResult> _awaiter;

        public EnumerableTask(List<TResult> result)
        {
            _result = result;
            _awaiter = new EnumerableTaskAwaiter<TResult>(this);
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public EnumerableTaskAwaiter<TResult> GetAwaiter()
        {
            return _awaiter;
        }
    }
}
