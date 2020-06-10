using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace TaskLike
{
    public class AsyncEnumerableTaskMethodBuilder<TResult>
    {
        public static AsyncEnumerableTaskMethodBuilder<TResult> Create() =>
            new AsyncEnumerableTaskMethodBuilder<TResult>();

        private Stack<EnumerableAwaiter<TResult>> _stack = new Stack<EnumerableAwaiter<TResult>>();

        public EnumerableTask<TResult> Task { get; } = new EnumerableTask<TResult>(new List<TResult>());

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

        public void SetException(Exception exception) { }

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
            if (!(awaiter is EnumerableAwaiter<TResult> lla)) return;

            if (_stack.Contains(lla)) return;

            _stack.Push(lla);

            var fields = stateMachine.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            var st = stateMachine;
            var state = SaveState(fields, st);

            while (!lla.MoveNext())
            {
                LoadState(fields, st, state);

                do
                {
                    stateMachine.MoveNext();
                }
                while ((int)fields[0].GetValue(stateMachine) != -2);
            }

            lla.Reset();
            _stack.Pop();
        }

        private static void LoadState<TStateMachine>(List<FieldInfo> fields, TStateMachine st, object[] state) where TStateMachine : IAsyncStateMachine
        {
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                field.SetValue(st, state[i]);
            }
        }

        private static object[] SaveState<TStateMachine>(List<FieldInfo> fields, TStateMachine st) where TStateMachine : IAsyncStateMachine
        {
            return fields.Select(f => f.GetValue(st)).ToArray();
        }
    }
}
