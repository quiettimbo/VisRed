using StackExchange.Redis;

namespace VisRed
{
    public interface IRedisContextProvider
    {
        ConnectionMultiplexer RedisService { get; }
    }
}