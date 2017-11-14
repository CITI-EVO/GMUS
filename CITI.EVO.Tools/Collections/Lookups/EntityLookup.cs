using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections.Lookups
{
    [Serializable]
    public class EntityLookup<TKey, TElement> : IEnumerable<KeyValuePair<TKey, TElement>>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly int _capacity;

        private Entity[] _entities;
        private Entity _lastEnt;

        private int _count;

        public EntityLookup()
            : this(EqualityComparer<TKey>.Default)
        {
        }
        public EntityLookup(int capacity)
            : this(EqualityComparer<TKey>.Default, capacity)
        {
        }
        public EntityLookup(IEqualityComparer<TKey> comparer)
            : this(comparer, 7)
        {
        }
        public EntityLookup(IEqualityComparer<TKey> comparer, int capacity)
        {
            if (comparer == null)
                comparer = EqualityComparer<TKey>.Default;

            _capacity = capacity;
            _comparer = comparer;
            _entities = new Entity[capacity];
        }

        public int Count
        {
            get { return _count; }
        }

        public TElement this[TKey key]
        {
            get
            {
                var entity = GetEntity(key, false);
                if (entity != null)
                    return entity.Element;

                return default(TElement);
            }
            set
            {
                var entity = GetEntity(key, true);
                entity.Element = value;
            }
        }

        public void Add(TKey key, TElement value)
        {
            var entity = GetEntity(key, true);
            entity.Element = value;
        }

        public bool Remove(TKey key)
        {
            var hashCode = GetHashCode(key);
            var index = hashCode % _entities.Length;

            var parentEntity = (Entity)null;
            var entity = _entities[index];

            while (entity != null)
            {
                if (entity.HashCode == hashCode && _comparer.Equals(entity.Key, key))
                {
                    if (parentEntity == null)
                        _entities[index] = null;
                    else
                    {
                        parentEntity.NextEntity = entity.NextEntity;
                        parentEntity.HashNext = entity.HashNext;
                    }

                    _count--;

                    return true;
                }

                parentEntity = entity;
                entity = entity.HashNext;
            }

            return false;
        }

        public bool Contains(TKey key)
        {
            return GetEntity(key, false) != null;
        }

        public void Clear()
        {
            _entities = new Entity[_capacity];
            _count = 0;
        }

        public bool TryGetValue(TKey key, out TElement value)
        {
            value = default(TElement);

            var entity = GetEntity(key, false);
            if (entity == null)
                return false;

            value = entity.Element;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TElement>> GetEnumerator()
        {
            var entity = _lastEnt;
            if (entity != null)
            {
                do
                {
                    entity = entity.NextEntity;

                    var pair = new KeyValuePair<TKey, TElement>(entity.Key, entity.Element);
                    yield return pair;

                } while (entity != _lastEnt);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Entity GetEntity(TKey key, bool create)
        {
            var hashCode = GetHashCode(key);
            var index = hashCode % _entities.Length;

            for (var entity = _entities[index]; entity != null; entity = entity.HashNext)
            {
                if (entity.HashCode == hashCode && _comparer.Equals(entity.Key, key))
                    return entity;
            }

            if (!create)
                return null;

            if (_count >= _entities.Length)
            {
                IncreaseSize();
                index = hashCode % _entities.Length;
            }

            var newEntity = new Entity
            {
                Key = key,
                HashCode = hashCode,
                HashNext = _entities[index]
            };

            _entities[index] = newEntity;

            if (_lastEnt == null)
            {
                newEntity.NextEntity = newEntity;
            }
            else
            {
                newEntity.NextEntity = _lastEnt.NextEntity;
                _lastEnt.NextEntity = newEntity;
            }

            _lastEnt = newEntity;
            _count++;

            return newEntity;
        }

        private int GetHashCode(TKey key)
        {
            if (key == null)
                return 0;

            return _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        private void IncreaseSize()
        {
            var newSize = checked(_count * 2 + 1);
            var newEntities = new Entity[newSize];

            var entity = _lastEnt;

            do
            {
                entity = entity.NextEntity;

                var index = entity.HashCode % newSize;
                entity.HashNext = newEntities[index];

                newEntities[index] = entity;
            } while (entity != _lastEnt);

            _entities = newEntities;
        }

        [Serializable]
        private class Entity
        {
            public TKey Key;
            public TElement Element;

            public int HashCode;

            public Entity HashNext;
            public Entity NextEntity;
        }
    }
}
