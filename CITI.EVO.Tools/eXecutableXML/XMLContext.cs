using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.eXecutableXML
{
    public class XMLContext
    {
        private readonly XMLContext _parentContent;
        private readonly IDictionary<String, Object> _dictionary;

        public XMLContext()
        {
            _dictionary = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);
        }
        public XMLContext(XMLContext parentContent)
        {
            _parentContent = parentContent;
            _dictionary = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);
        }

        public void SetValue(String name, Object value)
        {
            if (TrySet(name, value))
                return;

            _dictionary[name] = value;
        }

        public Object GetValue(String name)
        {
            Object value;
            if (_dictionary.TryGetValue(name, out value))
                return value;

            if (_parentContent != null)
                return _parentContent.GetValue(name);

            return null;
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        private bool TrySet(String name, Object value)
        {
            if (_dictionary.ContainsKey(name))
            {
                _dictionary[name] = value;
                return true;
            }

            if (_parentContent != null)
                return _parentContent.TrySet(name, value);

            return false;
        }
    }
}