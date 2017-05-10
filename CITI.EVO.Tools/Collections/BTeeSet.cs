using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CITI.EVO.Tools.Structures;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class BTeeSet<TItem> : ISet<TItem>
    {
        private readonly BTree<TItem> _btree;

        public BTeeSet() : this(Comparer<TItem>.Default)
        {
        }
        public BTeeSet(IComparer<TItem> comparer)
        {
            _btree = new BTree<TItem>(comparer);
        }

        public int Count
        {
            get { return _btree.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var item in _btree.InOrderTraversalItems())
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Add(TItem item)
        {
            return _btree.Add(item);
        }

        void ICollection<TItem>.Add(TItem item)
        {
            Add(item);
        }

        public void UnionWith(IEnumerable<TItem> other)
        {
            foreach (var item in other)
                Add(item);
        }

        public void IntersectWith(IEnumerable<TItem> other)
        {
            var list = new List<TItem>(Count);

            foreach (var item in other)
            {
                if (Contains(item))
                {
                    list.Add(item);
                    Remove(item);
                }
            }

            Clear();

            foreach (var item in list)
                Add(item);
        }

        public void ExceptWith(IEnumerable<TItem> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Count == 0)
                return;

            if (ReferenceEquals(other, this))
            {
                Clear();
                return;
            }

            foreach (var item in other)
                Remove(item);
        }

        public void SymmetricExceptWith(IEnumerable<TItem> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Count == 0)
                return;

            var list = new List<TItem>();
            foreach (var item in other)
            {
                if (Contains(item))
                    list.Add(item);
                else
                    Add(item);
            }

            foreach (var item in list)
                Remove(item);
        }

        public bool IsSubsetOf(IEnumerable<TItem> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Count == 0)
                return false;

            foreach (var item in other)
            {
                if (!Contains(item))
                    return false;
            }

            return true;
        }

        public bool IsSupersetOf(IEnumerable<TItem> other)
        {
            foreach (var item in other)
            {
                if (!Contains(item))
                    return false;
            }

            return true;
        }

        public bool IsProperSupersetOf(IEnumerable<TItem> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Count == 0)
                return false;

            foreach (var item in other)
            {
                if (!Contains(item))
                    return false;
            }

            return true;
        }

        public bool IsProperSubsetOf(IEnumerable<TItem> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Count == 0)
                return false;

            foreach (var item in this)
            {
                if (!other.Contains(item))
                    return false;
            }

            return true;
        }

        public bool Overlaps(IEnumerable<TItem> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (Count == 0)
                return false;

            foreach (var item in other)
            {
                if (Contains(item))
                    return true;
            }

            return false;
        }

        public bool SetEquals(IEnumerable<TItem> other)
        {
            var coll = other as ICollection<TItem>;
            if (coll != null)
            {
                if (coll.Count != Count)
                    return false;

                foreach (var item in other)
                {
                    if (!Contains(item))
                        return false;
                }

                return true;
            }
            else
            {
                var otherCount = 0;

                foreach (var item in other)
                {
                    otherCount++;

                    if (!Contains(item))
                        return false;
                }

                if (otherCount != Count)
                    return false;

                return true;
            }
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                if (arrayIndex >= array.Length)
                    break;

                array[arrayIndex++] = item;
            }
        }

        public bool Contains(TItem item)
        {
            return _btree.Contains(item);
        }

        public bool Remove(TItem item)
        {
            return _btree.Remove(item);
        }

        public void Clear()
        {
            _btree.Clear();
        }
    }
}
