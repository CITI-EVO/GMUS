using System;
using System.Collections;
using System.Collections.Generic;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataBaseList : IList<FormDataBase>
    {
        protected int? _count;
        protected IList<FormDataBase> _list;

        public FormDataBaseList()
        {
            _list = new List<FormDataBase>();
        }

        public FormDataBaseList(IEnumerable<FormDataBase> source)
        {
            _list = new List<FormDataBase>(source);
        }


        public FormDataBase this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public IEnumerator<FormDataBase> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(FormDataBase item)
        {
            _list.Add(item);
        }

        public void AddRange(IEnumerable<FormDataBase> items)
        {
            foreach (var item in items)
                _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(FormDataBase item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(FormDataBase[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(FormDataBase item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(FormDataBase item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, FormDataBase item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Sort(IComparer<FormDataBase> comparer)
        {
            var list = (List<FormDataBase>)_list;
            list.Sort(comparer);
        }
    }
}