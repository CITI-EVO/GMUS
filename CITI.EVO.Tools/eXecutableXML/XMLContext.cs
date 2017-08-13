using System;
using System.Collections.Generic;
using CITI.EVO.Tools.ExpressionEngine;

namespace CITI.EVO.Tools.eXecutableXML
{
    public class XMLContext : DefaultDataResolver
    {
        public XMLContext()
        {
        }

        public XMLContext(IDataResolver parenResolver) 
            : base(parenResolver)
        {
        }
    }
}