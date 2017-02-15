using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class DictionaryItemDescriptor : CustomTypeDescriptor
    {
        private readonly Type _type;
        private readonly IDictionary<String, Object> _item;
        private readonly PropertyDescriptorCollection _propertyDescriptors;

        public DictionaryItemDescriptor(IDictionary<String, Object> item, Type type, PropertyDescriptorCollection propertyDescriptors)
        {
            _item = item;
            _type = type;
            _propertyDescriptors = propertyDescriptors;
        }

        public Object Item
        {
            get { return _item; }
        }

        public Object GetValue(String key)
        {
            Object val;
            if (_item.TryGetValue(key, out val))
                return val;

            return null;
        }

        public void SetValue(String key, Object value)
        {
            _item[key] = value;
        }

        public void ResetValue(String key)
        {
            SetValue(key, null);
        }

        public override String GetClassName()
        {
            return _type.FullName;
        }

        public override String GetComponentName()
        {
            return _type.Name;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return _propertyDescriptors;
        }

        public Type GetDataType(MemberInfo memberInfo)
        {
            return typeof(Object);
        }
    }
}