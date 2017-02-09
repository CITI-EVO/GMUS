using System;
using System.ComponentModel;
using System.Reflection;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class CollectionItemPropertyDescriptor : PropertyDescriptor
    {
        private readonly MemberInfo _memberInfo;

        public CollectionItemPropertyDescriptor(MemberInfo memberInfo)
            : this(memberInfo, null)
        {
        }
        public CollectionItemPropertyDescriptor(MemberInfo memberInfo, Attribute[] attrs)
            : base(memberInfo.Name, attrs)
        {
            _memberInfo = memberInfo;
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
            var descriptor = (CollectionItemDescriptor)component;
            return descriptor.GetValue(_memberInfo);
        }

        public override void ResetValue(object component)
        {
            var descriptor = (CollectionItemDescriptor)component;
            descriptor.ResetValue(_memberInfo);
        }

        public override void SetValue(object component, object value)
        {
            var descriptor = (CollectionItemDescriptor)component;
            descriptor.SetValue(_memberInfo, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public Type GetDataType()
        {
            var fieldInfo = _memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            var propInfo = _memberInfo as PropertyInfo;
            if (propInfo != null)
            {
                return propInfo.PropertyType;
            }

            var methodInfo = _memberInfo as MethodInfo;
            if (methodInfo != null)
            {
                return methodInfo.ReturnType;
            }

            return null;
        }
    }
}