using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskLike
{
    public class ListLike<TResult> 
    {
        internal readonly IEnumerable<TResult> _result;
        ListLikeAwaiter<TResult> _awaiter;

        public ListLike(IEnumerable<TResult> result)
        {
            _result = result;
            _awaiter = new ListLikeAwaiter<TResult>(_result);
        }

        public bool IsCompleted { get; private set; }

        public ListLikeAwaiter<TResult> GetAwaiter()
        {
            return _awaiter;
        }
    }
}
