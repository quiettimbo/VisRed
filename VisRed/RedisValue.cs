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

    public class RedisNullValue : RedisVal
    {
        public RedisNullValue():base(RedisValue.Null, RedisType.None)
        {
        }
    }
}
