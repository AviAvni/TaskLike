using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskLike
{
    [AsyncMethodBuilder(typeof(AsyncTaskLikeMethodBuilder<>))]
    public class TaskLike<TResult> 
    {
        internal readonly List<TResult> _result;
        TaskLikeAwaiter<TResult> _awaiter;

        public TaskLike(List<TResult> result)
        {
            _result = result;
            _awaiter = new TaskLikeAwaiter<TResult>(this);
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public TaskLikeAwaiter<TResult> GetAwaiter()
        {
            return _awaiter;
        }
    }
}
