using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class MaxHeap<TItem> : IEnumerable<TItem>
    {
        private readonly IComparer<TItem> _comparer;

        private int _count;
        private TItem[] _array;

        public MaxHeap()
            : this(Comparer<TItem>.Default)
        {
        }
        public MaxHeap(IComparer<TItem> comparer)
        {
            _comparer = comparer;

            _array = new TItem[8];
            _count = 0;
        }

        public MaxHeap(IEnumerable<TItem> collection)
            : this(Comparer<TItem>.Default)
        {
            foreach (var item in collection)
            {
                Push(item);
            }
        }

        public MaxHeap(IComparer<TItem> comparer, IEnumerable<TItem> collection)
            : this(comparer)
        {
            foreach (var item in collection)
            {
                Push(item);
            }
        }

        private MaxHeap(MaxHeap<TItem> heap)
        {
            _count = heap._count;
            _comparer = heap._comparer;

            _array = new TItem[heap._array.Length];

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = heap._array[i];
            }
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public int Count
        {
            get { return _count; }
        }

        public TItem Peek()
        {
            if (IsEmpty())
                throw new Exception("Priority queue underflow");

            return _array[1];
        }

        public void Push(TItem x)
        {
            if (_count >= _array.Length - 1)
                Resize(2 * _array.Length);

            _array[++_count] = x;
            Swim(_count);
        }

        public TItem Pop()
        {
            if (IsEmpty())
                throw new Exception("Priority queue underflow");

            var max = _array[1];

            Exch(1, _count--);
            Sink(1);

            _array[_count + 1] = default(TItem);

            if ((_count > 0) && (_count == (_array.Length - 1) / 4))
                Resize(_array.Length / 2);

            return max;
        }

        private void Resize(int capacity)
        {
            var temp = new TItem[capacity];

            for (int i = 1; i <= _count; i++)
                temp[i] = _array[i];

            _array = temp;
        }

        private void Swim(int index)
        {
            var x = index;
            var y = x / 2;

            while (x > 1 && Less(y, x))
            {
                Exch(x, y);

                x = y;
                y /= 2;
            }
        }

        private void Sink(int index)
        {
            var x = index;
            var y = x * 2;

            while (y <= _count)
            {
                if (y < _count && Less(y, y + 1))
                    y++;

                if (!Less(x, y))
                    break;

                Exch(x, y);

                x = y;
                y *= 2;
            }
        }

        private bool Less(int i, int j)
        {
            return _comparer.Compare(_array[i], _array[j]) < 0;
        }

        private void Exch(int i, int j)
        {
            var t = _array[i];
            _array[i] = _array[j];
            _array[j] = t;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            var heap = new MaxHeap<TItem>(this);

            while (heap.Count > 0)
            {
                yield return heap.Pop();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}