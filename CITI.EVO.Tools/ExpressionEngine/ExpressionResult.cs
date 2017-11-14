using System;

namespace CITI.EVO.Tools.ExpressionEngine
{
    [Serializable]
    public class ExpressionResult
    {
        public Object Value { get; set; }
        public Exception Error { get; set; }
    }
}
