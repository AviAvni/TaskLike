using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLike
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var list = Enumerable.Range(0, 10);
            var res = await DoSomething(new ListLike<int>(list), new ListLike<int>(list), new ListLike<int>(list));

            var q = (from i in Enumerable.Range(0, 10)
                     from j in Enumerable.Range(0, 10)
                     from k in Enumerable.Range(0, 10)
                     select i * 100 + j * 10 + k).ToList();

            Console.WriteLine(res.SequenceEqual(q));
        }

        static async TaskLike<int> DoSomething(ListLike<int> list, ListLike<int> list1, ListLike<int> list2)
        {
            var i = await list;
            var s1 = i;
            var j = await list1;
            var s2 = s1 * 10 + j;
            var k = await list2;
            var res = s2 * 10 + k;

            await Task.Delay(100);

            var t = await Task.FromResult(res);

            Console.WriteLine($"{i}{j}{k}, {t}");

            return res;
        }
    }
}
