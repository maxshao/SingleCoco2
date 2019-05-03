using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Infrastructure
{
    public class RedisStore
    {

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisStore()
        {
            var configurationOptions = new ConfigurationOptions()
            {
                EndPoints = { "127.0.0.1", "6379" },
                Password = "foobared",
                KeepAlive = 1,
            };
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public static IDatabase RedisCache => Connection.GetDatabase();

    }
}
