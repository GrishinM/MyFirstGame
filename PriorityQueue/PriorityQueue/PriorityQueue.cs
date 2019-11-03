using System.Collections.Generic;
using System.Linq;

namespace PriorityQueue
{
    public class PriorityQueue<T> where T : class
    {
        private readonly SortedDictionary<float, Queue<T>> queue = new SortedDictionary<float, Queue<T>>();

        public void Add(float p, T value)
        {
            if (!queue.ContainsKey(p))
                queue.Add(p, new Queue<T>());
            queue[p].Enqueue(value);
        }

        public T Get()
        {
            var k = queue.Values.ToList();
            k.Reverse();
            foreach (var q in k)
                if (q.Count > 0)
                    return q.Dequeue();
            return null;
        }

        public IEnumerable<T> Values()
        {
            var ans=new List<T>();
            var k = queue.Values.ToList();
            k.Reverse();
            foreach (var q in k)
            {
                ans.AddRange(q.ToList());
            }

            return ans;
        }
    }
}