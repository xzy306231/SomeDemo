using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var commandMap = new Dictionary<string, Action>
        {
            { "Greet", () => Console.WriteLine("Hello!") },
            { "Farewell", () => Console.WriteLine("Goodbye!") },
            { "Thank", () => Console.WriteLine("Thank you!") }
        };

            // 执行命令
            ExecuteCommand(commandMap, "Greet");
            ExecuteCommand(commandMap, "Farewell");


            List<List<int>> data = new List<List<int>> { new List<int> { 3, 2 }, new List<int> { 4, 3 }, new List<int> { 2, 6 }, new List<int> { 6, 3 } };
        //    int[][] d = [ [ 3, 2 ], [4, 3 ], [ 2, 6], [ 6, 3 ] ];
            GetStaleServerCount(6, data, new int[] {3,2,6 }, 2);
            getstaleserverCount(6, data, new List<int> { 3, 2, 6 }, 2);
            var ll = new List<int> { 1, 2, 3, 4, 6 };
            var s = ll.BinarySearch(5);
            if (s < 0)
            {
                s = ~s;
            }
            StackMethod();
            QueueMethod();
            var b = 141 & 108;
            //  var i = LengthOfLongestSubstring("abcabcbb");
            //var j = LengthOfLongestSubstring("bbbbb");
            timeInWords(5, 47);
            var k = LengthOfLongestSubstring("pwwkew");
            //string s = "fvbnmhjklghj";
            //var array = s.ToCharArray();
            //for (int i = 0; i < s.Length; i++)
            //{
            //    Console.Write($"{s[i]}");
            //}
            //Console.WriteLine("======================");
            //for (int i = 0; i < array.Length; i++)
            //{
            //    Console.Write($"{array[i]}");
            //}
            //Console.WriteLine("======================");
            //for (int i = array.Length - 1; i >= 0; i--)
            //{
            //    Console.Write($"{array[i]}");
            //}
            //Console.WriteLine("======================");
            ////   array=array.Reverse().ToArray();
            //for (int i = 0; i < array.Length; i++)
            //{
            //    Console.Write($"{array[i]}");
            //}


            //var i=FibonacciSequence(100);
            //foreach (var item in FibonacciSequence(10))
            //{
            //    Console.Write($"-------{item}\n");
            //}
            //var i = Factorial(5);
            //TryParseToInt("789", out int num);
            //   Console.WriteLine($"Hello, World!{i}");
            ListNode list1 = new ListNode { val = 1, next = new ListNode { val = 2, next = new ListNode { val = 4 } } };
            ListNode list2 = new ListNode { val = 2, next = new ListNode { val = 3, next = new ListNode { val = 4 } } };
            var node = MergeTwoLists(list1, list2);
            Console.ReadKey();
        }

        public static void ExecuteCommand(Dictionary<string, Action> commandMap, string command)
        {
            if (commandMap.ContainsKey(command))
            {
                commandMap[command].Invoke();
            }
            else
            {
                Console.WriteLine("未知命令");
            }
        }

        static List<int> GetStaleServerCount(int n, List<List<int>> log_data, int[] query, int x)
        {
            List<int> results = new List<int>();
            foreach (int q in query)
            {
                int lower_bound = q - x;
                HashSet<int> active_servers = new HashSet<int>();
                foreach (var log in log_data)
                {
                    if (lower_bound <= log[1] && log[1] <= q)
                    {
                        active_servers.Add(log[0]);
                    }
                }
                int stale_servers_count = n - active_servers.Count;
                results.Add(stale_servers_count);
            }
            return results;
        }
        public static List<int> getstaleserverCount(int n, List<List<int>> log_data, List<int> query, int x)
        {
            Dictionary<int, List<int>> sd = new Dictionary<int, List<int>>();
            foreach (var l in log_data)
            {
                if (!sd.ContainsKey(l[0]))
                    sd[l[0]] = new List<int>();
                sd[l[0]].Add(l[1]);
            }
            foreach (var key in sd.Keys)
                sd[key].Sort();
            List<int> res = new List<int>();
            foreach (var q in query)
            {
                int cnt = 0;
                foreach (var kv in sd)
                {
                    var lst = kv.Value;
                    int l = q - x;
                    int r = q;
                    int idx = lst.BinarySearch(l);
                    if (idx < 0)
                        idx = ~idx;
                    bool f = false;
                    while (idx < lst.Count && lst[idx] <= r)
                    {
                        f = true;
                        break;
                    }
                    if (!f)
                        cnt++;
                }
                res.Add(n - (sd.Count - cnt));
            }
            return res;
        }
        private static void StackMethod()
        {
            Stack<string> stack = new Stack<string>();

            //将元素入栈
            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            //栈的元素个数
            int count = stack.Count;
            //是否包含指定的元素
            bool b = stack.Contains("a");

            //Stack.Peek() 方法返回顶部的对象而不将其从堆栈中移除
            string name = stack.Peek();

            // Pop 把元素出栈，栈中就没有这个元素了
            string s1 = stack.Pop();
            Console.WriteLine(s1);
            string s2 = stack.Pop();
            Console.WriteLine(s2);
            string s3 = stack.Pop();
            Console.WriteLine(s3);

            Console.ReadKey();
        }
        private static void QueueMethod()
        {
            Queue<string> queue = new Queue<string>();

            //向队列中添加元素
            queue.Enqueue("老一");
            queue.Enqueue("老二");
            queue.Enqueue("老三");

            //获取队列的数量
            int count = queue.Count;
            //队列中是否包含指定的 value
            bool b = queue.Contains("老王");

            //Peek方法是返回顶部的对象而不将其从堆栈中移除
            string names = queue.Peek();

            //获取队列中的元素
            //每次调用 Dequeue 方法，获取并移除队列中队首的元素
            string s1 = queue.Dequeue();
            Console.WriteLine(s1);
            string s2 = queue.Dequeue();
            Console.WriteLine(s2);
            string s3 = queue.Dequeue();
            Console.WriteLine(s3);

            //清空队列
            queue.Clear();


            Console.ReadKey();

        }
        public static string timeInWords(int h, int m)
        {
            string[] ones = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "", "", "twenty", "thirty", "forty", "fifty" };

            if (m == 0)
            {
                return ones[h] + " o' clock";
            }
            else if (m == 15)
            {
                return "quarter past " + ones[h];
            }
            else if (m == 30)
            {
                return "half past " + ones[h];
            }
            else if (m == 45)
            {
                return "quarter to " + ones[(h % 12) + 1];
            }
            else if (m == 1)
            {
                return ones[m] + " minute past " + ones[h];
            }
            else if (m < 20)
            {
                return ones[m] + " minutes past " + ones[h];
            }
            else
            {

                int tensPart = m / 10;
                int onesPart = m % 10;
                string time = tens[tensPart];
                if (m > 30 && m < 60 && m != 45)
                {
                    // time = ones[60 - m];
                }
                // if (onesPart > 0)
                // {
                //     time += " " + ones[onesPart];
                // }
                if (m < 30)
                {

                    // time+=ones[onesPart];
                    // time+= " minutes past " + ones[h];
                    return tens[tensPart] + ones[onesPart] + " minutes past " + ones[h];
                }
                else
                {
                    m = 60 - m;
                    if (m > 20)
                    {
                        tensPart = m / 10;
                        onesPart = m % 10;
                        return tens[tensPart] + " " + ones[onesPart] + " minutes to " + ones[(h % 12) + 1];
                    }
                    else
                    {
                        return ones[m] + " minutes to " + ones[(h % 12) + 1];
                    }
                    //time += " minutes to " + ones[(h % 12) + 1];
                }
                return time;
            }
        }


        public static ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if (l1 == null)
            {
                return l2;
            }
            else if (l2 == null)
            {
                return l1;
            }
            else if (l1.val < l2.val)
            {
                l1.next = MergeTwoLists(l1.next, l2);
                return l1;
            }
            else
            {
                l2.next = MergeTwoLists(l1, l2.next);
                return l2;
            }
        }
        public static int LengthOfLongestSubstring(string s)
        {
            var array = s.ToCharArray();
            var dict = new Dictionary<char, int>();
            int res = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (!dict.ContainsKey(array[i]))
                {
                    dict.Add(array[i], i);
                    res++;
                }
                else
                {

                }

            }

            return res;

        }

        public static IEnumerable<int> FibonacciSequence(int count)
        {
            int a = 0, b = 1;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"================{i}===========");
                yield return a;
                int temp = a;
                a = b;
                b = temp + b;
            }
        }
        public static bool TryParseToInt(string strData, out int num)
        {
            if (string.IsNullOrWhiteSpace(strData))
            {
                num = 0;
                return false;
            }
            int result = 0;

            bool minus = strData[0] == '-' ? true : false;
            if (minus && strData.Length == 1)
            {
                num = 0;
                return false;
            }

            for (int i = minus ? 1 : 0; i < strData.Length; i++)
            {
                if (strData[i] >= '0' && strData[i] <= '9')
                {
                    result = strData[i] - 48 + result * 10;
                }
                else
                {
                    num = 0;
                    return false;
                }
            }

            num = minus ? -result : result;
            return true;
        }
        public static int Factorial(int n)
        {
            if (n == 0 || n == 1)
            {
                return 1;
            }
            else
            {
                // 递归调用：当前数n乘以前面所有数的阶乘
                return n * Factorial(n - 1);
            }
        }
    }

    //Definition for singly-linked list.
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }

}
