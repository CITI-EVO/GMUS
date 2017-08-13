using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Helpers
{
    public class StringTokenizer : StringTokenizerOpt
    {
        public StringTokenizer(String text, params char[] separators) : this(text, new HashSet<char>(separators))
        {
        }
        public StringTokenizer(String text, IEnumerable<char> separators) : this(text, new HashSet<char>(separators))
        {
        }
        public StringTokenizer(String text, ISet<char> separators) : base(separators)
        {
            InitEnumerator(text);
        }
    }
}