using StackExchange.Redis;
using System;

namespace VisRed
{
    public class RedisServer 
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public static RedisVal RedisFactory(IDatabase db, RedisKey key)
        {
            switch(db.KeyType(key))
            {
                case RedisType.String:
                    return new RedisStringValue(db.StringGet(key));
                case RedisType.Hash:
                    return new RedisHashValue(db.HashGetAll(key));
                case RedisType.List:
                    return new RedisListValue(db.ListRange(key));
                case RedisType.Set:
                    return new RedisSetValue(db.SetMembers(key));
                case RedisType.SortedSet:
                    return new RedisSortedSetValue(db.SortedSetRangeByRank(key));
                default:
                    return new RedisNullValue();
            }
        }
    }

}