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
            var res = await DoSomething(new ListLike<int>(list), new ListLike<int>(list));

            var q = (from i in Enumerable.Range(0, 10)
                     from j in Enumerable.Range(0, 10)
                     select i * 10 + j).ToList();

            Console.WriteLine(res.SequenceEqual(q));
            //while (!task.IsCompleted)
            //{
            //    Console.WriteLine(string.Join(", ", task._result));
            //    await Task.Delay(10000);
            //}
        }

        static async TaskLike<int> DoSomething(ListLike<int> list, ListLike<int> list1)
        {
            var i = await list;
            var j = await list1;
            Console.WriteLine($"{i}, {j}");
            //await Task.Delay(1000);
            return i * 10 + j;
        }
    }
}
