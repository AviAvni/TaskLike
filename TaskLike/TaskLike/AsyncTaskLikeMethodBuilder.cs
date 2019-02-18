using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;

namespace TaskLike
{
    public class AsyncTaskLikeMethodBuilder<TResult>
    {
        public static AsyncTaskLikeMethodBuilder<TResult> Create() =>
            new AsyncTaskLikeMethodBuilder<TResult>();

        private Stack<ListLikeAwaiter<TResult>> _stack = new Stack<ListLikeAwaiter<TResult>>();

        public TaskLike<TResult> Task { get; } = new TaskLike<TResult>(new List<TResult>());

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void SetResult(TResult result)
        {
            Task._result.Add(result);
        }

        public void SetException(Exception exception) => throw exception;

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            // Doesn't seem to be used under normal circumstances

            // awaiter.OnCompleted(stateMachine.MoveNext);
        }


        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (!(awaiter is ListLikeAwaiter<TResult> lla)) return;

            if (_stack.Contains(lla)) return;

            _stack.Push(lla);

            if (_stack.Count > 1)
            {
                lla.MoveNext();
                return;
            }

            var st = stateMachine;
            var field = st.GetType().GetFields()[0];
            var times = -2;

            lla.UnsafeOnCompleted(() =>
            {
                while (_stack.Count > 0)
                {
                    while (!lla.MoveNext())
                    {
                        st.MoveNext();

                        while ((int)field.GetValue(st) != times)
                            st.MoveNext();

                        lla = _stack.Peek();
                    }

                    _stack.Pop();
                    if (_stack.Count > 0)
                    {
                        lla.Reset();
                        lla = _stack.Peek();
                    }
                }
            });
        }
    }
}
