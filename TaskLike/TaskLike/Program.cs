using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLike
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var list = Enumerable.Range(0, 10);
            var res = await DoSomething(list, list, list);

            var q = (from i in Enumerable.Range(0, 10)
                     from j in Enumerable.Range(0, 10)
                     from k in Enumerable.Range(0, 10)
                     select i * 100 + j * 10 + k).ToList();

            Console.WriteLine(res.SequenceEqual(q));
        }

        static async EnumerableTask<int> DoSomething(IEnumerable<int> list, IEnumerable<int> list1, IEnumerable<int> list2)
        {
            var i = await list;
            var s1 = i;

            await Task.Delay(100);
            
            var j = await list1;
            var s2 = s1 * 10 + j;
            
            var k = await list2;
            
            var res = s2 * 10 + k;

            return res;
        }
    }
}
