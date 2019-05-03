using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleCoco.Infrastructure;
namespace L.Redis.RedisUtils
{
    class RedisString
    {
        public void Test()
        {

            var redis = RedisStore.RedisCache;

            //String types are the basic data types in Redis. Strings in Redis can store integers, images, files, strings and serialized objects, since Redis strings are byte arrays that are binary safe and have a maximum size of 512MB
            // key

            var key = "testKey";
            if (redis.StringSet(key, "testValue"))
            {
                var val = redis.StringGet(key);

                //output - StringGet(testKey) value is testValue
                Console.WriteLine("StringGet({0}) value is {1}", key, val);

                var v1 = redis.StringGetSet(key, "testValue2");

                //output - StringGetSet(testKey) testValue == testValue
                Console.WriteLine("StringGetSet({0}) {1} == {2}", key, val, v1);

                val = redis.StringGet(key);

                //output - StringGet(testKey) value is testValue2
                Console.WriteLine("StringGet({0}) value is {1}", key, val);

                //using SETNX 
                //code never goes into if since key already exist
                if (redis.StringSet(key, "someValue", TimeSpan.MaxValue, When.NotExists))
                {
                    val = redis.StringGet(key);
                    Console.WriteLine("StringGet({0}) value is {1}", key, val);
                }
                else
                {
                    //always goes here
                    Console.WriteLine("Value already exist");
                }

                var key2 = key + "1";
                if (redis.StringSet(key2, "someValue", TimeSpan.MaxValue, When.NotExists))
                {
                    val = redis.StringGet(key2);
                    //output - StringGet(testKey2) value is someValue", key2, val
                    Console.WriteLine("StringGet({0}) value is {1}", key2, val);
                }
                else
                {
                    //never goes here
                    Console.WriteLine("Value already exist");
                }
            }

            //Example of using multiple key-value in C#
            KeyValuePair<RedisKey, RedisValue>[] values = {
               new KeyValuePair<RedisKey, RedisValue>("a", "x"),
               new KeyValuePair<RedisKey, RedisValue>("b", "y")
            };
            if (redis.StringSet(values))
            {
                RedisKey[] myKeys = { "b", "a" };
                var allValues = redis.StringGet(myKeys);

                //output - y,x
                Console.WriteLine(string.Join(",", allValues));
            }



            //C# Code using integer and float
            var number = 101;
            var intKey = "intKey";
            if (redis.StringSet(intKey, number))
            {
                //redis incr command
                var result = redis.StringIncrement(intKey); //after operation Our int number is now 102
                Console.WriteLine(result);

                //incrby command
                var newNumber = redis.StringIncrement(intKey, 100); // we now have incremented by 100, thus the new number is 202
                Console.WriteLine(newNumber);

                redis.KeyDelete("zeroValueKey");
                //by default redis stores a value of zero if no value is provided           
                var zeroValue = (int)redis.StringGet("zeroValueKey");
                Console.WriteLine(zeroValue);

                var someValue = (int)redis.StringIncrement("zeroValueKey"); //someValue is now 1 since it was incremented
                Console.WriteLine(someValue);

                //decr command
                redis.StringDecrement("zeroValueKey");
                someValue = (int)redis.StringGet("zeroValueKey"); //now someValue is back to 0   
                Console.WriteLine(someValue);

                //decrby command
                someValue = (int)redis.StringDecrement("zeroValueKey", 99); // now someValue is -99   
                Console.WriteLine(someValue);

                //append command
                redis.StringAppend("zeroValueKey", 1);
                someValue = (int)redis.StringGet("zeroValueKey"); //"Our zeroValueKey number is now -991   
                Console.WriteLine(someValue);

                redis.StringSet("floatValue", 1.1);
                var floatValue = (float)redis.StringIncrement("floatValue", 0.1); //fload value is now 1.2   
                Console.WriteLine(floatValue);
            }


        }
    }
}
