using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class CollectionItemDescriptor : CustomTypeDescriptor
    {
        private readonly Type _type;
        private readonly Object _item;
        private readonly PropertyDescriptorCollection _propertyDescriptors;

        public CollectionItemDescriptor(Object item, Type type, PropertyDescriptorCollection propertyDescriptors)
        {
            _item = item;
            _type = type;
            _propertyDescriptors = propertyDescriptors;
        }

        public Object Item
        {
            get { return _item; }
        }

        public Object GetValue(MemberInfo memberInfo)
        {
            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(_item);
            }

            var propInfo = memberInfo as PropertyInfo;
            if (propInfo != null)
            {
                return propInfo.GetValue(_item);
            }

            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null)
            {
                return methodInfo.Invoke(_item, null);
            }

            return null;
        }

        public void SetValue(MemberInfo memberInfo, Object value)
        {
            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(_item, value);
            }

            var propInfo = memberInfo as PropertyInfo;
            if (propInfo != null)
            {
                propInfo.SetValue(_item, value);
            }

            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null)
            {
                methodInfo.Invoke(_item, new[] { value });
            }
        }

        public void ResetValue(MemberInfo memberInfo)
        {
            var @default = (Object)null;

            var type = GetDataType(memberInfo);
            if (type != null && type.IsValueType)
            {
                @default = Activator.CreateInstance(type);
            }

            SetValue(memberInfo, @default);
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
            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            var propInfo = memberInfo as PropertyInfo;
            if (propInfo != null)
            {
                return propInfo.PropertyType;
            }

            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null)
            {
                return methodInfo.ReturnType;
            }

            return null;
        }
    }
}