using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisRed
{
    public class RedisVal   
    {
        public RedisVal(RedisValue value, RedisType type)
        {
            _valueholder = new Lazy<RedisValue>(() => value);
            RedisType = type;
        }

        static public RedisVal ValueFactory(IDatabase db, RedisKey key)
        {
            var rtype = db.KeyType(key);
            switch (rtype)
            {
                case RedisType.String:
                    return new RedisStringValue(db, key);
                case RedisType.Hash:
                    return new RedisHashValue(db, key);
                case RedisType.List:
                    return new RedisListValue(db, key);
                case RedisType.Set:
                    return new RedisSetValue(db, key);
                case RedisType.SortedSet:
                    return new RedisSortedSetValue(db, key);
                default:
                    return new RedisNullValue();
            }
        }

        protected RedisVal(RedisType rtype)
        {
            RedisType = rtype;
        }

        public RedisType RedisType { get; set; }
        public RedisValue Value
        {
            get
            {
                return _valueholder.Value;
            }
            set
            {
                _valueholder = new Lazy<RedisValue>(() => value);
            }
        }

        protected Lazy<RedisValue> _valueholder;
    }

    public class RedisStringValue : RedisVal
    {
        public RedisStringValue(RedisValue value):base(value, RedisType.String)
        {

        }

        public RedisStringValue(IDatabase db, RedisKey key) : base(RedisType.String)
        {
            _valueholder = new Lazy<RedisValue>(() => db.StringGet(key));
        }

        public override string ToString()
        {
            return RedisType.ToString() + " " + Value.ToString();
        }
    }

    public class RedisHashValue : RedisVal
    {
        protected Lazy<HashEntry[]> _valuesHolder;
        public RedisHashValue(HashEntry[] value) : base( RedisType.Hash)
        {
            _valuesHolder = new Lazy<HashEntry[]>(() => value);
        }

        public RedisHashValue(IDatabase db, RedisKey key) : base(RedisType.Hash)
        {
            _valuesHolder = new Lazy<HashEntry[]>(() => db.HashGetAll(key));
        }

        public HashEntry[] Values
        {
            get
            {
                return _valuesHolder.Value;
            }
        }
    }

    public class RedisListValue : RedisVal
    {
        protected Lazy<RedisValue[]> _valuesHolder;
        public RedisListValue(RedisValue[] value) : base(RedisType.List)
        {
            _valuesHolder = new Lazy<RedisValue[]>(()=> value);
        }

        public RedisListValue(IDatabase db, RedisKey key) : base(RedisType.List)
        {
            _valuesHolder = new Lazy<RedisValue[]>(() => db.ListRange(key));
        }
        public RedisValue[] Values
        {
            get
            {
                return _valuesHolder.Value;
            }
        }
    }

    public class RedisSetValue : RedisVal
    {
        protected Lazy<RedisValue[]> _valuesHolder;
        public RedisSetValue(RedisValue[] value) : base(RedisType.Set)
        {
            _valuesHolder = new Lazy<RedisValue[]>(() => value);
        }

        public RedisSetValue(IDatabase db, RedisKey key) : base(RedisType.List)
        {
            _valuesHolder = new Lazy<RedisValue[]>(() => db.SetMembers(key));
        }

        public RedisValue[] Values
        {
            get
            {
                return _valuesHolder.Value;
            }
        }
    }

    public class RedisSortedSetValue : RedisVal
    {
        protected Lazy<RedisValue[]> _valuesHolder;
        public RedisSortedSetValue(RedisValue[] value) : base(RedisValue.Null, RedisType.SortedSet)
        {
            _valuesHolder = new Lazy<RedisValue[]>(()=>value);
        }

        public RedisSortedSetValue(IDatabase db, RedisKey key) : base(RedisType.List)
        {
            _valuesHolder = new Lazy<RedisValue[]>(() => db.SortedSetRangeByRank(key));
        }

        public RedisValue[] Values
        {
            get
            {
                return _valuesHolder.Value;
            }
        }
    }

    public class RedisNullValue : RedisVal
    {
        public RedisNullValue():base(RedisValue.Null, RedisType.None)
        {
        }
    }
}
