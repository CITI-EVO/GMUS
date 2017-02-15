using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class DictionaryDataView : ITypedList, IEnumerable<DictionaryItemDescriptor>
    {
        private readonly Type _type;
        private readonly PropertyDescriptorCollection _propertyDescriptors;
        private readonly IEnumerable<IDictionary<String, Object>> _collection;

        public DictionaryDataView(IEnumerable<IDictionary<String, Object>> collection, ISet<String> fields)
        {
            _type = typeof(IDictionary<String, Object>);
            _collection = collection;

            var descriptorsList = new List<PropertyDescriptor>();

            foreach (var key in fields)
            {
                var descriptor = new DictionaryItemPropertyDescriptor(key);
                descriptorsList.Add(descriptor);
            }

            var descriptorsArray = descriptorsList.ToArray();
            _propertyDescriptors = new PropertyDescriptorCollection(descriptorsArray, true);
        }

        public Type Type
        {
            get { return _type; }
        }

        public IEnumerable Collection
        {
            get { return _collection; }
        }

        public IEnumerator<DictionaryItemDescriptor> GetEnumerator()
        {
            foreach (var item in _collection)
                yield return new DictionaryItemDescriptor(item, _type, _propertyDescriptors);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return _propertyDescriptors;
        }

        public String GetListName(PropertyDescriptor[] listAccessors)
        {
            return _type.Name;
        }
    }
}