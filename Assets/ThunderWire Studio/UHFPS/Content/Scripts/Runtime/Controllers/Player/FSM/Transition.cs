using System;
using UHFPS.Scriptable;

namespace UHFPS.Runtime
{
    public sealed class Transition
    {
        public Type NextState { get; private set; }
        public Func<bool> Condition { get; private set; }
        public bool Value => Condition.Invoke();

        public static Transition To<TState>(Func<bool> condition) where TState : StateAsset
        {
            return new Transition()
            {
                NextState = typeof(TState),
                Condition = condition
            };
        }

        public static Transition Back(Func<bool> condition)
        {
            return new Transition()
            {
                NextState = null,
                Condition = condition
            };
        }
    }
}