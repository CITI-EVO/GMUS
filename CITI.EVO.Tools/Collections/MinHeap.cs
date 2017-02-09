using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    public class MinHeap<TItem> : IEnumerable<TItem>
    {
        private readonly IComparer<TItem> _comparer;

        private int _count;
        private TItem[] _array;

        public MinHeap()
            : this(Comparer<TItem>.Default)
        {
        }
        public MinHeap(IComparer<TItem> comparer)
        {
            _comparer = comparer;

            _array = new TItem[8];
            _count = 0;
        }

        public MinHeap(IEnumerable<TItem> collection)
            : this(Comparer<TItem>.Default)
        {
            foreach (var item in collection)
            {
                Push(item);
            }
        }

        public MinHeap(IComparer<TItem> comparer, IEnumerable<TItem> collection)
            : this(comparer)
        {
            foreach (var item in collection)
            {
                Push(item);
            }
        }

        private MinHeap(MinHeap<TItem> heap)
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

        public void Push(TItem item)
        {
            // double size of array if necessary
            if (_count == _array.Length - 1)
                Resize(2 * _array.Length);

            // add x, and percolate it up to maintain heap invariant
            _array[++_count] = item;

            Swim(_count);
        }

        public TItem Pop()
        {
            if (IsEmpty())
                throw new Exception("Priority queue underflow");

            Exch(1, _count);

            var item = _array[_count--];

            Sink(1);

            _array[_count + 1] = default(TItem);

            if ((_count > 0) && (_count == (_array.Length - 1) / 4))
                Resize(_array.Length / 2);

            return item;
        }

        private void Swim(int index)
        {
            var x = index;
            var y = x / 2;

            while (x > 1 && Greater(y, x))
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
                if (y < _count && Greater(y, y + 1))
                    y++;

                if (!Greater(x, y))
                    break;

                Exch(x, y);

                x = y;
                y *= 2;
            }
        }

        private void Resize(int capacity)
        {
            var temp = new TItem[capacity];

            for (int i = 1; i <= _count; i++)
                temp[i] = _array[i];

            _array = temp;
        }

        private bool Greater(int x, int y)
        {
            return _comparer.Compare(_array[x], _array[y]) > 0;
        }

        private void Exch(int x, int y)
        {
            var t = _array[x];

            _array[x] = _array[y];
            _array[y] = t;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            var heap = new MinHeap<TItem>(this);

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
