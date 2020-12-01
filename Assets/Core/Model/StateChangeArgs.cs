using System;

namespace NotTimeTravel.Core.Model
{
    public class StateChangeArgs<T> : EventArgs
    {
        public string PropertyName { get; set; }

        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }
}