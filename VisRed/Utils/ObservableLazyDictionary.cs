using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisRed.Utils
{
    public class ObservableLazyDictionary<TKey, TValue> : ObservableDictionary<TKey, TValue>
    {
        private LazyDictionary<TKey,TValue> Internal
        {
            get
            {
                return Dictionary as LazyDictionary<TKey, TValue>;
            }
        }
        #region Constructors
        public ObservableLazyDictionary():base(typeof(LazyDictionary<TKey, TValue>), new LazyDictionary<TKey, TValue>())
        { }
        public ObservableLazyDictionary(IDictionary<TKey, TValue> dictionary):base(typeof(LazyDictionary<TKey, TValue>),new LazyDictionary<TKey, TValue>(dictionary))
        {
        }
        public ObservableLazyDictionary(IEqualityComparer<TKey> comparer):base(typeof(LazyDictionary<TKey, TValue>), new LazyDictionary<TKey, TValue>(comparer))
        {
        }
        public ObservableLazyDictionary(int capacity):base(typeof(LazyDictionary<TKey, TValue>), new LazyDictionary<TKey, TValue>(capacity))
        {
        }
        public ObservableLazyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            :base(typeof(LazyDictionary<TKey, TValue>), new LazyDictionary<TKey, TValue>(dictionary, comparer))
        {
        }
        public ObservableLazyDictionary(int capacity, IEqualityComparer<TKey> comparer)
            :base(typeof(LazyDictionary<TKey, TValue>), new LazyDictionary<TKey, TValue>(capacity, comparer))
        {
        }
        #endregion

        public void Add(TKey key, Func<TValue> func)
        {
            Insert(key, func, true);
        }

        private void Insert(TKey key, Func<TValue> func, bool add)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue item;
            if (Dictionary.TryGetValue(key, out item))
            {
                if (add) throw new ArgumentException("An item with the same key has already been added.");
                Internal.Add(key, func);

                OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, default(TValue)), new KeyValuePair<TKey, TValue>(key, item));
            }
            else
            {
                Internal.Add(key, func);

                OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, default(TValue)));
            }
        }

    }
}
