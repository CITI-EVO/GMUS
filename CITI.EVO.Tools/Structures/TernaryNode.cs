using System;

namespace CITI.EVO.Tools.Structures
{
    [Serializable]
    public class TernaryNode<TValue>
    {
        public char Char { get; set; }

        public bool IsLeaf { get; set; }

        public String Key { get; set; }

        public TValue Value { get; set; }

        public TernaryNode<TValue> Left { get; set; }
        public TernaryNode<TValue> Middle { get; set; }
        public TernaryNode<TValue> Right { get; set; }
    }
}