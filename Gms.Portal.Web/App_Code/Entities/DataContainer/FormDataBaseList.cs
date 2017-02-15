using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataBaseList : IList<FormDataUnit>
    {
        private readonly List<FormDataUnit> _list;

        private FormDataBaseList()
        {
            _list = new List<FormDataUnit>();
        }

        public FormDataBaseList(Guid? formID) : this(formID, formID, null)
        {
        }
        public FormDataBaseList(Guid? formID, Guid? ownerID) : this(formID, ownerID, null)
        {
        }
        public FormDataBaseList(Guid? formID, Guid? ownerID, Guid? parentID) : this()
        {
            if (formID == null || ownerID == null)
                throw new Exception();

            FormID = formID;
            OwnerID = ownerID;
            ParentID = parentID;
        }


        public Guid? FormID { get; set; }

        public Guid? OwnerID { get; set; }

        public Guid? ParentID { get; set; }

        public FormDataUnit this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public virtual IEnumerator<FormDataUnit> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(FormDataUnit item)
        {
            _list.Add(item);
        }

        public void AddRange(IEnumerable<FormDataUnit> items)
        {
            _list.AddRange(items);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(FormDataUnit item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(FormDataUnit[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(FormDataUnit item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(FormDataUnit item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, FormDataUnit item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
    }
}