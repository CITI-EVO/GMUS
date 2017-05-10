using System;

namespace CITI.EVO.Tools.Web.Services
{
    public class DynamicObject
    {
        public DynamicObject(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            ObjectInstance = obj;
            ObjectType = obj.GetType();
        }

        public DynamicObject(Type objType)
        {
            if (objType == null)
                throw new ArgumentNullException("objType");

            ObjectType = objType;
        }

        public Type ObjectType { get; private set; }

        public Object ObjectInstance { get; private set; }

        //public BindingFlags BindingFlags { get; set; }

        public void CallConstructor()
        {
            CallConstructor(new Type[0], new object[0]);
        }

        public void CallConstructor(Type[] paramTypes, Object[] paramValues)
        {
            var ctor = ObjectType.GetConstructor(paramTypes);
            if (ctor == null)
                throw new DynamicProxyException(DynamicErrorMessages.ProxyCtorNotFound);

            ObjectInstance = ctor.Invoke(paramValues);
        }

        public Object GetProperty(String propertyName)
        {
            return GetProperty(propertyName, null);
        }
        public Object GetProperty(String propertyName, Object[] parameters)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            var propertyInfo = ObjectType.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new InvalidOperationException("Property not found");

            //return ObjectType.InvokeMember(property, BindingFlags.GetProperty | BindingFlags, null, ObjectInstance, null);
            return propertyInfo.GetValue(ObjectInstance, parameters);
        }

        public void SetProperty(String propertyName, Object value)
        {
            SetProperty(propertyName, value, null);
        }
        public void SetProperty(String propertyName, Object value, Object[] parameters)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            var propertyInfo = ObjectType.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new InvalidOperationException("Property not found");

            //return ObjectType.InvokeMember(property, BindingFlags.SetProperty | BindingFlags, null, ObjectInstance, new[] { value });
            propertyInfo.SetValue(ObjectInstance, value, parameters);
        }

        public Object GetField(String fieldName)
        {
            if (String.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            var fieldInfo = ObjectType.GetField(fieldName);
            if (fieldInfo == null)
                throw new InvalidOperationException("Field not found");

            return fieldInfo.GetValue(ObjectInstance);

            //return ObjectType.InvokeMember(field, BindingFlags.GetField | BindingFlags, null, ObjectInstance, null);
        }

        public void SetField(String fieldName, Object value)
        {
            if (String.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            var fieldInfo = ObjectType.GetField(fieldName);
            if (fieldInfo == null)
                throw new InvalidOperationException("Field not found");

            fieldInfo.SetValue(ObjectInstance, value);

            //return ObjectType.InvokeMember(field, BindingFlags.SetField | BindingFlags, null, ObjectInstance, new[] { value });
        }

        public Object CallMethod(String methodName)
        {
            return CallMethod(methodName, null);
        }
        public Object CallMethod(String methodName, Object[] parameters)
        {
            if (String.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            var methodInfo = ObjectType.GetMethod(methodName);
            if (methodInfo == null)
                throw new InvalidOperationException("Method not found");

            return methodInfo.Invoke(ObjectInstance, parameters);
            //return ObjectType.InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags, null, ObjectInstance, parameters);
        }
        public Object CallMethod(String methodName, Type[] types, Object[] parameters)
        {
            if (types.Length != parameters.Length)
                throw new ArgumentException(DynamicErrorMessages.ParameterValueMistmatch);

            if (String.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");

            var methodInfo = ObjectType.GetMethod(methodName, types);
            if (methodInfo == null)
                throw new ApplicationException(String.Format(DynamicErrorMessages.MethodNotFound, methodName));

            return methodInfo.Invoke(ObjectInstance, parameters);
        }
    }
}