using System;
using System.Collections;
using System.Collections.Generic;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataListBase : IList<FormDataUnit>
    {
        protected int? _count;
        protected IList<FormDataUnit> _list;

        protected FormDataListBase()
        {
        }

        public FormDataListBase(FormDataListBase formDataBaseList)
        {
            UserID = formDataBaseList.UserID;
            FormID = formDataBaseList.FormID;
            OwnerID = formDataBaseList.OwnerID;
            ParentID = formDataBaseList.ParentID;

            _count = formDataBaseList._count;

            if (formDataBaseList._list != null)
                _list = new List<FormDataUnit>(formDataBaseList._list);
        }

        public FormDataListBase(Guid? formID) : this(formID, formID, null, null)
        {
        }
        public FormDataListBase(Guid? formID, Guid? ownerID) : this(formID, ownerID, null, null)
        {
        }
        public FormDataListBase(Guid? formID, Guid? ownerID, Guid? parentID) : this(formID, ownerID, parentID, null)
        {
        }
        public FormDataListBase(Guid? formID, Guid? ownerID, Guid? parentID, Guid? userID) : this()
        {
            if (formID == null || ownerID == null)
                throw new Exception();

            UserID = userID;
            FormID = formID;
            OwnerID = ownerID;
            ParentID = parentID;
        }

        public Guid? FormID { get; set; }

        public Guid? OwnerID { get; set; }

        public Guid? ParentID { get; set; }

        public Guid? UserID { get; set; }

        public FormDataUnit this[int index]
        {
            get
            {
                InitItems();
                return _list[index];
            }
            set
            {
                InitItems();
                _list[index] = value;
            }
        }

        public IEnumerator<FormDataUnit> GetEnumerator()
        {
            InitItems();
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(FormDataUnit item)
        {
            InitItems();
            _list.Add(item);
        }

        public void AddRange(IEnumerable<FormDataUnit> items)
        {
            InitItems();

            foreach (var item in items)
                _list.Add(item);
        }

        public void Clear()
        {
            InitItems();
            _list.Clear();
        }

        public bool Contains(FormDataUnit item)
        {
            InitItems();
            return _list.Contains(item);
        }

        public void CopyTo(FormDataUnit[] array, int arrayIndex)
        {
            InitItems();
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(FormDataUnit item)
        {
            InitItems();
            return _list.Remove(item);
        }

        public int Count
        {
            get
            {
                if (_list == null)
                {
                    InitCount();
                    return _count.GetValueOrDefault();
                }

                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(FormDataUnit item)
        {
            InitItems();
            return _list.IndexOf(item);
        }

        public void Insert(int index, FormDataUnit item)
        {
            InitItems();
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InitItems();
            _list.RemoveAt(index);
        }

        private void InitCount()
        {
            if (_count == null)
                _count = InitializeCount();
        }
        private void InitItems()
        {
            if (_list == null)
                _list = InitializeItems();
        }

        protected virtual int InitializeCount()
        {
            return 0;
        }

        protected virtual IList<FormDataUnit> InitializeItems()
        {
            return new List<FormDataUnit>();
        }
    }
}