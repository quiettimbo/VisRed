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
            Value = value;
            RedisType = type;
        }

        public RedisType RedisType { get; set; }
        public RedisValue Value { get; set; }
    }

    public class RedisStringValue : RedisVal
    {
        public RedisStringValue(RedisValue value):base(value, RedisType.String)
        {

        }
        public override string ToString()
        {
            return RedisType.ToString() + " " + Value.ToString();
        }
    }

    public class RedisHashValue : RedisVal
    {
        protected HashEntry[] _values;
        public RedisHashValue(HashEntry[] value) : base(RedisValue.Null, RedisType.Hash)
        {
            _values = value;
        }

        public HashEntry[] Values
        {
            get
            {
                return _values;
            }
        }
    }

    public class RedisListValue : RedisVal
    {
        protected RedisValue[] _values;
        public RedisListValue(RedisValue[] value) : base(RedisValue.Null, RedisType.List)
        {
            _values = value;
        }

        public RedisValue[] Values
        {
            get
            {
                return _values;
            }
        }
    }

    public class RedisSetValue : RedisVal
    {
        protected RedisValue[] _values;
        public RedisSetValue(RedisValue[] value) : base(RedisValue.Null, RedisType.Set)
        {
            _values = value;
        }

        public RedisValue[] Values
        {
            get
            {
                return _values;
            }
        }
    }

    public class RedisSortedSetValue : RedisVal
    {
        protected RedisValue[] _values;
        public RedisSortedSetValue(RedisValue[] value) : base(RedisValue.Null, RedisType.SortedSet)
        {
            _values = value;
        }

        public RedisValue[] Values
        {
            get
            {
                return _values;
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
