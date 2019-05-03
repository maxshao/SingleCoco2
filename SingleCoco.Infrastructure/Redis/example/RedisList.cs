using SingleCoco.Infrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Redis.RedisUtils
{
    public class RedisList
    {
        public static void Test()
        {


            var redis = RedisStore.RedisCache;

            var listKey = "listKey";

            redis.KeyDelete(listKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "a");

            var len = redis.ListLength(listKey);
            Console.WriteLine(len); //output  is 1

            redis.ListRightPush(listKey, "b");

            Console.WriteLine(redis.ListLength(listKey)); //putput is 2

            //lets clear it out
            redis.KeyDelete(listKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "abcdefghijklmnopqrstuvwxyz".Select(x => (RedisValue)x.ToString()).ToArray());

            Console.WriteLine(redis.ListLength(listKey)); //output is 26

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output is abcdefghijklmnopqrstuvwxyz

            var lastFive = redis.ListRange(listKey, -5);

            Console.WriteLine(string.Concat(lastFive)); //output vwxyz
            // 获取索范围内的数据
            var firstFive = redis.ListRange(listKey, 0, 4);

            Console.WriteLine(string.Concat(firstFive)); //output abcde
            //刪除這個範圍内的所有數值
            redis.ListTrim(listKey, 0, 1);

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output ab

            //lets clear it out
            redis.KeyDelete(listKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "abcdefghijklmnopqrstuvwxyz".Select(x => (RedisValue)x.ToString()).ToArray());
            // 取出来第一个存进去的数据，先进先出？
            var firstElement = redis.ListLeftPop(listKey);

            Console.WriteLine(firstElement); //output a, list is now bcdefghijklmnopqrstuvwxyz
            // 取出最后一个存进去的数据，先进后出？
            var lastElement = redis.ListRightPop(listKey);

            Console.WriteLine(lastElement); //output z, list is now bcdefghijklmnopqrstuvwxy
            // 移除序列中指定的数据
            redis.ListRemove(listKey, "c");

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output is bdefghijklmnopqrstuvwxy   
            // 把指定数据插入到指定的索引
            redis.ListSetByIndex(listKey, 1, "c");

            Console.WriteLine(string.Concat(redis.ListRange(listKey))); //output is bcefghijklmnopqrstuvwxy   
            // 获取指定索引的数据
            var thirdItem = redis.ListGetByIndex(listKey, 3);

            Console.WriteLine(thirdItem); //output f  

            //lets clear it out
            var destinationKey = "destinationList";
            redis.KeyDelete(listKey, CommandFlags.FireAndForget);
            redis.KeyDelete(destinationKey, CommandFlags.FireAndForget);

            redis.ListRightPush(listKey, "abcdefghijklmnopqrstuvwxyz".Select(x => (RedisValue)x.ToString()).ToArray());

            var listLength = redis.ListLength(listKey);

            for (var i = 0; i < listLength; i++)
            {
                // 把当前数据按照倒序插入到另一个list中
                var val = redis.ListRightPopLeftPush(listKey, destinationKey);

                Console.Write(val);    //output zyxwvutsrqponmlkjihgfedcba
            }

            Console.ReadKey();


        }

    }
}
