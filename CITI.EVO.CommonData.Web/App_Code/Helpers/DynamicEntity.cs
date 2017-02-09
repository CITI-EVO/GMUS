using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CITI.EVO.CommonData.Web.Helpers
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
// The class derived from DynamicObject. 
    public class DynamicEntity : DynamicObject
    {
        private readonly Object obj;

        // The inner dictionary.
        private readonly IDictionary<String, PropertyInfo> dictionary;

        public DynamicEntity(Object obj)
        {
            this.obj = obj;

            var type = obj.GetType();

            dictionary = type.GetProperties().ToDictionary(n => n.Name);
        }

        public override bool TryGetMember(GetMemberBinder binder, out Object result)
        {
            var propertyInfo = dictionary[binder.Name];
            result = propertyInfo.GetValue(obj, null);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, Object value)
        {
            var propertyInfo = dictionary[binder.Name];
            propertyInfo.SetValue(obj, value);

            return true;
        }
    }
}