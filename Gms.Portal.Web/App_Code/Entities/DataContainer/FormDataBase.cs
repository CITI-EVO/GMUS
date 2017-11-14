using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MongoDB.Bson;

namespace Gms.Portal.Web.Entities.DataContainer
{
    public class FormDataBase : IDictionary<String, Object>
    {
        private static readonly String[] _defaultFieldsArr =
        {
            FormDataConstants.IDField,
            FormDataConstants.DocIDField,
            FormDataConstants.FormIDField,
            FormDataConstants.UserIDField,
            FormDataConstants.OwnerIDField,
            FormDataConstants.IDNumberField,
            FormDataConstants.StatusIDField,
            FormDataConstants.ParentIDField,
            FormDataConstants.PreviousIDField,
            FormDataConstants.ContainerIDField,
            FormDataConstants.ParentVersionField,

            FormDataConstants.VersionField,
            FormDataConstants.HashCodeField,

            FormDataConstants.ReviewFields,
            FormDataConstants.PrivacyFields,
            FormDataConstants.DescriptionField,

            FormDataConstants.DateOfStatusField,

            FormDataConstants.DateCreatedField,
            FormDataConstants.DateChangedField,
            FormDataConstants.DateDeletedField,

            FormDataConstants.DateOfAcceptField,
            FormDataConstants.DateOfSubmitField,
            FormDataConstants.DateOfAssigneField,

            FormDataConstants.ChangesRequiresAcceptField
        };

        private static ISet<String> _defaultFields;
        public static ISet<String> DefaultFields
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                _defaultFields = (_defaultFields ?? new HashSet<String>(_defaultFieldsArr));
                return _defaultFields;
            }
        }

        private readonly Dictionary<String, Object> _dict;

        public FormDataBase() : this(true)
        {
        }
        public FormDataBase(bool ignoreCaseComparer)
        {
            var comparer = StringComparer.Ordinal;
            if (ignoreCaseComparer)
                comparer = StringComparer.OrdinalIgnoreCase;

            _dict = new Dictionary<String, Object>(comparer)
            {
                [FormDataConstants.DocIDField] = ObjectId.GenerateNewId()
            };
        }

        public Object this[String key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection<String> Keys
        {
            get { return _dict.Keys; }
        }

        public ICollection<Object> Values
        {
            get { return _dict.Values; }
        }

        public void Add(String key, Object value)
        {
            _dict.Add(key, value);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool ContainsKey(String key)
        {
            return _dict.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            var defaultKeys = DefaultFields;

            foreach (var key in defaultKeys)
            {
                var value = GetValue(key);
                yield return new KeyValuePair<String, Object>(key, value);
            }

            foreach (var pair in _dict)
            {
                if (defaultKeys.Contains(pair.Key))
                    continue;

                yield return pair;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(String key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(String key, out Object value)
        {
            return _dict.TryGetValue(key, out value);
        }

        protected Object GetValue(String key)
        {
            Object val;
            if (TryGetValue(key, out val))
                return val;

            return null;
        }

        protected void SetValue(String key, Object val)
        {
            _dict[key] = val;
        }

        void ICollection<KeyValuePair<String, Object>>.Add(KeyValuePair<String, Object> item)
        {
            Add(item.Key, item.Value);
        }
        bool ICollection<KeyValuePair<String, Object>>.Contains(KeyValuePair<String, Object> item)
        {
            return ContainsKey(item.Key);
        }
        void ICollection<KeyValuePair<String, Object>>.CopyTo(KeyValuePair<String, Object>[] array, int arrayIndex)
        {
            foreach (var item in _dict)
            {
                if (arrayIndex++ >= array.Length)
                    break;

                array[arrayIndex] = item;
            }
        }
        bool ICollection<KeyValuePair<String, Object>>.Remove(KeyValuePair<String, Object> item)
        {
            return Remove(item.Key);
        }
    }
}