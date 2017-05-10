using System;

namespace CITI.EVO.Tools.eXecutableXML
{
    public class XMLReturn : XMLResult
    {
        public XMLReturn(Object value)
        {
            Value = value;
        }

        public Object Value { get; private set; }
    }
}