using System;
using System.Collections;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Collections
{
    [Serializable]
    public class EntityLookup<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly int _capacity;

        private Entity[] _entities;
        private Entity _lastEntity;

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

        public TValue this[TKey key]
        {
            get
            {
                var entity = GetEntity(key, false);
                if (entity != null)
                    return entity.Value;

                return default(TValue);
            }
            set
            {
                var entity = GetEntity(key, true);
                entity.Value = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            var entity = GetEntity(key, true);
            entity.Value = value;
        }

        public bool Remove(TKey key)
        {
            var hashCode = GetHashCode(key);
            var index = hashCode % _entities.Length;

            var parentGroup = (Entity)null;

            for (var group = _entities[index]; group != null; parentGroup = group, group = group.HashNext)
            {
                if (group.HashCode == hashCode && _comparer.Equals(group.Key, key))
                {
                    if (parentGroup == null)
                    {
                        _entities[index] = null;
                    }
                    else
                    {
                        parentGroup.NextEntity = group.NextEntity;
                        parentGroup.HashNext = group.HashNext;
                    }

                    return true;
                }
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

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            var entity = GetEntity(key, false);
            if (entity == null)
                return false;

            value = entity.Value;
            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var entity = _lastEntity;
            if (entity != null)
            {
                do
                {
                    entity = entity.NextEntity;

                    var pair = new KeyValuePair<TKey, TValue>(entity.Key, entity.Value);
                    yield return pair;

                } while (entity != _lastEntity);
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

            if (create)
            {
                if (_count >= _entities.Length)
                {
                    EnsureCapacity();
                    index = hashCode % _entities.Length;
                }

                var entity = new Entity
                {
                    Key = key,
                    HashCode = hashCode,
                    HashNext = _entities[index]
                };

                _entities[index] = entity;

                if (_lastEntity == null)
                {
                    entity.NextEntity = entity;
                }
                else
                {
                    entity.NextEntity = _lastEntity.NextEntity;
                    _lastEntity.NextEntity = entity;
                }

                _lastEntity = entity;
                _count++;

                return entity;
            }

            return null;
        }

        private int GetHashCode(TKey key)
        {
            if (key == null)
                return 0;

            return _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        private void EnsureCapacity()
        {
            var newSize = checked(_count * 2 + 1);
            var newEntities = new Entity[newSize];

            var entity = _lastEntity;

            do
            {
                entity = entity.NextEntity;

                var index = entity.HashCode % newSize;
                entity.HashNext = newEntities[index];

                newEntities[index] = entity;
            } while (entity != _lastEntity);

            _entities = newEntities;
        }

        [Serializable]
        private class Entity
        {
            private TKey _key;
            private TValue _value;
            private int _hashCode;

            private Entity _hashNext;
            private Entity _nextEntity;

            public TKey Key
            {
                get { return _key; }
                set { _key = value; }
            }

            public TValue Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public int HashCode
            {
                get { return _hashCode; }
                set { _hashCode = value; }
            }

            public Entity HashNext
            {
                get { return _hashNext; }
                set { _hashNext = value; }
            }

            public Entity NextEntity
            {
                get { return _nextEntity; }
                set { _nextEntity = value; }
            }
        }
    }
}
