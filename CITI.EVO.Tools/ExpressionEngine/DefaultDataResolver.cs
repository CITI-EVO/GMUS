using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.ExpressionEngine
{
    public class DefaultDataResolver : IDataResolver
    {
        private readonly IDataResolver _parentResolver;
        private readonly IDictionary<String, Object> _dictionary;

        public DefaultDataResolver()
            : this(new Dictionary<String, Object>())
        {
        }
        public DefaultDataResolver(IDictionary<String, Object> dictionary)
            : this(dictionary, null)
        {
        }
        public DefaultDataResolver(IDataResolver parentResolver)
            : this(new Dictionary<String, Object>(), parentResolver)
        {
        }
        public DefaultDataResolver(IDictionary<String, Object> dictionary, IDataResolver parentResolver)
        {
            _dictionary = dictionary;
            _parentResolver = parentResolver;
        }

        public Object this[String name]
        {
            get { return GetValue(name); }
            set { SetValue(name, value); }
        }

        public virtual Object GetValue(String name)
        {
            Object value;
            if (TryGetValue(name, out value))
                return value;

            return null;
        }

        public virtual bool TryGetValue(String name, out Object value)
        {
            if (_dictionary.TryGetValue(name, out value))
                return true;

            if (_parentResolver != null)
                return _parentResolver.TryGetValue(name, out value);

            return false;
        }

        public virtual bool SetValue(String name, Object value)
        {
            if (!_dictionary.ContainsKey(name) && _parentResolver != null && _parentResolver.Contains(name))
                return _parentResolver.SetValue(name, value);

            _dictionary[name] = value;
            return true;
        }

        public virtual bool TrySetValue(String name, Object value)
        {
            if (Contains(name))
            {
                SetValue(name, value);
                return true;
            }

            return false;
        }

        public virtual bool Contains(String name)
        {
            if (_dictionary.ContainsKey(name))
                return true;

            if (_parentResolver != null)
                return _parentResolver.Contains(name);

            return false;
        }

        public virtual bool Remove(String name)
        {
            if (_dictionary.Remove(name))
                return true;

            if (_parentResolver != null)
                return _parentResolver.Remove(name);

            return false;
        }

        public virtual void Clear()
        {
            _dictionary.Clear();
        }
    }
}
