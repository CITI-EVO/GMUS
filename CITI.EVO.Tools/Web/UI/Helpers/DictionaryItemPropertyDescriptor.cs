using System;
using System.ComponentModel;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class DictionaryItemPropertyDescriptor : PropertyDescriptor
    {
        private readonly String _key;

        public DictionaryItemPropertyDescriptor(String key)
            : this(key, null)
        {
        }
        public DictionaryItemPropertyDescriptor(String key, Attribute[] attrs)
            : base(key, attrs)
        {
            _key = key;
        }

        public override Type ComponentType
        {
            get { return typeof(CollectionItemDescriptor); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return GetDataType(); }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            var descriptor = (DictionaryItemDescriptor)component;
            return descriptor.GetValue(_key);
        }

        public override void ResetValue(object component)
        {
            var descriptor = (DictionaryItemDescriptor)component;
            descriptor.ResetValue(_key);
        }

        public override void SetValue(object component, object value)
        {
            var descriptor = (DictionaryItemDescriptor)component;
            descriptor.SetValue(_key, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public Type GetDataType()
        {
            return typeof(Object);
        }
    }
}