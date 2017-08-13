using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CITI.EVO.Tools.Helpers
{
    public class StringTokenizerOpt : IEnumerable<String>, IEnumerator<String>
    {
        private IEnumerator<char> _enumumerator;

        private readonly StringBuilder _buffer;
        private readonly ISet<char> _separators;

        public StringTokenizerOpt(params char[] separators) : this(new HashSet<char>(separators))
        {
        }
        public StringTokenizerOpt(IEnumerable<char> separators) : this(new HashSet<char>(separators))
        {
        }
        public StringTokenizerOpt(ISet<char> separators)
        {
            _buffer = new StringBuilder();
            _separators = separators;
        }

        public StringTokenizerOpt Tokenize(String text)
        {
            InitEnumerator(text);
            return this;
        }

        public IEnumerator<String> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void InitEnumerator(String text)
        {
            _enumumerator = text.GetEnumerator();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            _buffer.Clear();

            var flag = false;

            while (_enumumerator.MoveNext())
            {
                var @char = _enumumerator.Current;
                if (_separators.Contains(@char))
                {
                    Current = _buffer.ToString();
                    flag = true;
                    break;
                }

                _buffer.Append(@char);
            }

            if (_buffer.Length > 0)
            {
                Current = _buffer.ToString();
                flag = true;
            }

            return flag;
        }

        public void Reset()
        {
            _enumumerator.Reset();
        }

        public String Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
