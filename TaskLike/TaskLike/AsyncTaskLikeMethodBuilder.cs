using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace TaskLike
{
    public class AsyncTaskLikeMethodBuilder<TResult>
    {
        public static AsyncTaskLikeMethodBuilder<TResult> Create() =>
            new AsyncTaskLikeMethodBuilder<TResult>();

        private AsyncTaskMethodBuilder<TResult> _methodBuilder = AsyncTaskMethodBuilder<TResult>.Create();
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
            awaiter.OnCompleted(stateMachine.MoveNext);
        }


        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is ListLikeAwaiter<TResult> lla)
            {
                if (!_stack.Contains(lla)) _stack.Push(lla);
                else return;

                lla.Reset();

                while (!lla.MoveNext())
                {
                    lla.UnsafeOnCompleted(stateMachine.MoveNext);
                    var field = stateMachine.GetType().GetFields()[0];
                    while ((int)field.GetValue(stateMachine) != -2)
                    {
                        lla.UnsafeOnCompleted(stateMachine.MoveNext);
                    }
                }

                _stack.Pop();
                return;
            }
            //_methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
        }
    }
}
