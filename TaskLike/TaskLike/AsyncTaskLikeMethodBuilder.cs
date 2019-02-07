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

            while (!lla.MoveNext())
            {
                var field = stateMachine.GetType().GetFields()[0];
                stateMachine.MoveNext();

                var times =
#if DEBUG
                        -2;
#else
                        _stack.Count - 1;
#endif

                while ((int)field.GetValue(stateMachine) != times)
                    stateMachine.MoveNext();
            }

            _stack.Pop();

            if (_stack.Count > 0)
                lla.Reset();
        }
    }
}
