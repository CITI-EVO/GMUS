using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CITI.EVO.Tools.Web.UI.Helpers
{
    public class CollectionDataView : ITypedList, IEnumerable<CollectionItemDescriptor>
    {
        private readonly Type _type;
        private readonly IEnumerable _collection;
        private readonly PropertyDescriptorCollection _propertyDescriptors;

        private CollectionDataView(IEnumerable collection, Type type, ISet<String> fields)
        {
            _type = type;
            _collection = collection;

            var allMembers = new HashSet<MemberInfo>();

            if (type.IsInterface)
            {
                var stack = new Stack<Type>();
                stack.Push(type);

                while (stack.Count > 0)
                {
                    var current = stack.Pop();

                    var members = current.GetMembers();
                    allMembers.UnionWith(members);

                    var parents = current.GetInterfaces();
                    foreach (var parent in parents)
                    {
                        stack.Push(parent);
                    }
                }
            }
            else
            {
                var members = type.GetMembers();
                allMembers.UnionWith(members);
            }

            var descriptorsList = new List<PropertyDescriptor>();

            foreach (var member in allMembers)
            {
                if (fields != null && !fields.Contains(member.Name))
                    continue;

                var descriptor = new CollectionItemPropertyDescriptor(member);
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

        public IEnumerator<CollectionItemDescriptor> GetEnumerator()
        {
            foreach (var item in _collection)
            {
                yield return new CollectionItemDescriptor(item, _type, _propertyDescriptors);
            }
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

        public static CollectionDataView Create<TItem>(IEnumerable collection)
        {
            return Create(collection, typeof(TItem), null);
        }
        public static CollectionDataView Create<TItem>(IEnumerable collection, ISet<String> fields)
        {
            return Create(collection, typeof(TItem), fields);
        }

        public static CollectionDataView Create(IEnumerable collection, Type type)
        {
            return Create(collection, type, null);
        }
        public static CollectionDataView Create(IEnumerable collection, Type type, ISet<String> fields)
        {
            return new CollectionDataView(collection, type, fields);
        }

        public static IEnumerable<TItem> GetDataSource<TItem>(Object source)
        {
            var collectionDataView = source as CollectionDataView;
            return GetDataSource<TItem>(collectionDataView);
        }
        public static IEnumerable<TItem> GetDataSource<TItem>(CollectionDataView source)
        {
            var collection = GetDataSource(source) as IEnumerable<TItem>;
            return collection;
        }
        public static IEnumerable GetDataSource(Object source)
        {
            var collectionDataView = source as CollectionDataView;
            return GetDataSource(collectionDataView);
        }
        public static IEnumerable GetDataSource(CollectionDataView source)
        {
            if (source != null)
            {
                return source.Collection;
            }

            return null;
        }
    }
}