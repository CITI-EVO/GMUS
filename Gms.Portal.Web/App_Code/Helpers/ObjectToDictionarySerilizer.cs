using System;
using System.Collections;
using System.Collections.Generic;

namespace Gms.Portal.Web.Helpers
{
    public class ObjectToDictionarySerilizer
    {
        public ObjectToDictionarySerilizer()
        {
        }

        public IDictionary<String, Object> Serialize(Object obj)
        {
            var result = new Dictionary<String, Object>();

            if (obj == null)
                return result;

            if (IsPrimitive(obj))
            {
                result["@"] = obj;
                return result;
            }

            if (!IsPrimitive(obj) && obj is IEnumerable)
            {
                var list = new List<Object>();
                var collection = (IEnumerable)obj;

                foreach (var item in collection)
                {
                    if (IsPrimitive(item))
                        list.Add(item);
                    else
                    {
                        var subDict = Serialize(item);
                        list.Add(subDict);
                    }
                }

                result["@"] = list;
                return result;
            }

            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                var propertyName = propertyInfo.Name;
                var propertyValue = propertyInfo.GetValue(obj);

                if (IsPrimitive(propertyValue))
                    result[propertyName] = propertyValue;
                else
                    result[propertyName] = Serialize(propertyValue);
            }

            return result;
        }

        private bool IsPrimitive(Object obj)
        {
            if (obj == null ||
                obj is String ||
                obj is byte ||
                obj is sbyte ||
                obj is short ||
                obj is ushort ||
                obj is int ||
                obj is uint ||
                obj is long ||
                obj is ulong ||
                obj is float ||
                obj is double ||
                obj is decimal ||
                obj is DateTime ||
                obj is TimeSpan ||
                obj is Guid)
                return true;

            return false;
        }

        
    }
}