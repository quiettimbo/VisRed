using StackExchange.Redis;
using System;

namespace VisRed
{
    public class RedisServer 
    {
        public string Url { get; set; }

        public static RedisVal RedisFactory(IDatabase db, RedisKey key)
        {
            switch(db.KeyType(key))
            {
                case RedisType.String:
                    return new RedisStringValue(db.StringGet(key));
                default:
                    return new RedisNullValue();
            }
        }
    }

}