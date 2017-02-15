using System;

namespace CITI.EVO.Tools.EventArguments
{
    public class GenericEventArgs<TValue> : EventArgs
    {
        public GenericEventArgs(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; private set; }
    }
}