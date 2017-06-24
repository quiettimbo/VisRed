using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisRed.Utils
{
    class LazyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        protected Dictionary<TKey, Lazy<TValue>> _dict;

        #region Constructors
        public LazyDictionary()
        {
            _dict = new Dictionary<TKey, Lazy<TValue>>();
        }

        public LazyDictionary(IDictionary<TKey, TValue>source):this()
        {
            InitElements(source);
        }

        public LazyDictionary(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, Lazy<TValue>>(comparer);
        }

        public LazyDictionary(int capacity)
        {
            _dict = new Dictionary<TKey, Lazy<TValue>>(capacity);
        }

        public LazyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            :this(comparer)
        {
            InitElements(dictionary);
        }

        public LazyDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, Lazy<TValue>>(capacity, comparer);
        }

        #endregion
        public TValue this[TKey key]
        {
            get
            {
                var lazy = _dict[key];
                return lazy != null ? lazy.Value : default(TValue);
            }

            set
            {
                _dict[key] = new Lazy<TValue>(()=> value);
            }
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return IsReadOnly;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _dict.Values.Select(val=>val.Value).ToArray();
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dict.Add(item.Key, new Lazy<TValue>(()=>item.Value));
        }

        public void Add(TKey key, TValue value)
        {
            _dict.Add(key, new Lazy<TValue>(() => value));
        }

        public void Add(TKey key, Func<TValue> func)
        {
            _dict.Add(key, new Lazy<TValue>(func));
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            Lazy<TValue> val;
            if (_dict.TryGetValue(item.Key, out val))
            {
                return item.Value.Equals(val.Value);
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int ii = arrayIndex;
            foreach(var item in this)
            {
                array[ii++] = item;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new LazyEnumerator(_dict.GetEnumerator());
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Lazy<TValue> lazy;
            if (_dict.TryGetValue(key, out lazy))
            {
                value = lazy == null ? default(TValue) : lazy.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void InitElements(IDictionary<TKey, TValue> source)
        {
            foreach (var item in source)
            {
                _dict.Add(item.Key, new Lazy<TValue>(() => item.Value));
            }
        }

        private class LazyEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            IEnumerator<KeyValuePair<TKey, Lazy<TValue>>> _internal;
            KeyValuePair<TKey, TValue> _current;

            public LazyEnumerator(IEnumerator<KeyValuePair<TKey,Lazy<TValue>>> dict)
            {
                _internal = dict;
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return _current.Equals(default(KeyValuePair<TKey,TValue>) )
                        ? _current = new KeyValuePair<TKey, TValue>(_internal.Current.Key, _internal.Current.Value.Value) 
                        : _current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public void Dispose()
            {
                if (_internal != null)
                {
                    _internal.Dispose();
                    _internal = null;
                }
            }

            public bool MoveNext()
            {
                _current = default(KeyValuePair<TKey, TValue>);
                return _internal.MoveNext();
            }

            public void Reset()
            {
                _current = default(KeyValuePair<TKey, TValue>);
                _internal.Reset();
            }
        }
    }
}
