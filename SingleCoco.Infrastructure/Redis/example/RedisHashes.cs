using SingleCoco.Infrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Redis.RedisUtils
{
    public class RedisHashes
    {

        public static void Test()
        {

            var redis = RedisStore.RedisCache;


            var hashKey = "hashKey";

            HashEntry[] redisBookHash = {
                            new HashEntry("title", "Redis for .NET Developers"),
                            new HashEntry("year", 2016),
                            new HashEntry("author", "Taswar Bhatti")
                          };

            redis.HashSet(hashKey, redisBookHash);

            if (redis.HashExists(hashKey, "year"))
            {
                var year = redis.HashGet(hashKey, "year"); //year is 2016
            }

            var allHash = redis.HashGetAll(hashKey);

            //get all the items
            foreach (var item in allHash)
            {
                //output 
                //key: title, value: Redis for .NET Developers
                //key: year, value: 2016
                //key: author, value: Taswar Bhatti
                Console.WriteLine(string.Format("key : {0}, value : {1}", item.Name, item.Value));
            }

            //get all the values
            var values = redis.HashValues(hashKey);

            foreach (var val in values)
            {
                Console.WriteLine(val); //result = Redis for .NET Developers, 2016, Taswar Bhatti
            }

            //get all the keys
            var keys = redis.HashKeys(hashKey);

            foreach (var k in keys)
            {
                Console.WriteLine(k); //result = title, year, author
            }

            var len = redis.HashLength(hashKey);  //result of len is 3

            if (redis.HashExists(hashKey, "year"))
            {
                var year = redis.HashIncrement(hashKey, "year", 1); //year now becomes 2017
                var year2 = redis.HashDecrement(hashKey, "year", 1.5); //year now becomes 2015.5
            }


        }




    }
}
