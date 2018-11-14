using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab16
{
    class Program
    {
        static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        static CancellationToken token = cancelTokenSource.Token;
        static BlockingCollection<string> collection;

        static void Producer(string product)
        {
            collection.Add(product);
            Console.WriteLine($"Товар {product} выставлен на продажу");
        }

        static void Consumer()
        {
            if(collection.TryTake(out string product))
            {
                Console.WriteLine($"Покупатель приобрёл {product}");
            }
            else
            {
                Console.WriteLine("Покупатель ушёл с пустыми руками");
            }
        }

        static void Main(string[] args)
        {


            //Task1_2();
            //Task3();          
            //Task4();
            //Task5();      
            //Task6();



            var watch = Stopwatch.StartNew();
            collection = new BlockingCollection<string>();

            Task[] producers = new Task[5];
            producers[0] = Task.Run(() => Producer("MK X"));
            producers[1] = Task.Run(() => Producer("Don't Starve"));
            producers[2] = Task.Run(() => Producer("Long Dark"));
            producers[3] = Task.Run(() => Producer("Oxygen not included"));
            producers[4] = Task.Run(() => Producer("Frotpunk"));

            Task[] consumers = new Task[]
            {
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer()),
                new Task(() => Consumer())
            };

            Task.WaitAll(producers);

            foreach (var c in consumers)
            {
                c.Start();
            }


            Task.Factory.ContinueWhenAll(consumers.ToArray(),
                result =>
                {
                    Console.WriteLine($"Выполнено за {watch.ElapsedMilliseconds.ToString()} мс");
                });

            Console.ReadLine();

        }

        public static void Task1_2()
        {
             

            Task task = new Task(PrimeNum);
            task.Start();

            Console.WriteLine($"ID: { task.Id.ToString()}, Status: {task.Status}, Completed: {task.IsCompleted} ");



            //Task 2           

            string s = Console.ReadLine();
            while (true)
            {
                if (s == "s")
                {
                    cancelTokenSource.Cancel();
                    Console.WriteLine($"ID: { task.Id.ToString()}, Status: {task.Status}, Completed: {task.IsCompleted} ");
                    break;
                }
            }

            Thread.Sleep(500);
            Console.WriteLine($"ID: { task.Id.ToString()}, Status: {task.Status}, Completed: {task.IsCompleted} ");
        }

        public static void Task3()
        {
              int i;
              int j;

        Task<int> task1 = new Task<int>(() => i = 10);
            Task<int> task2 = new Task<int>(() => j = task1.Result + 5);
            Task<int> task3 = new Task<int>(() => task2.Result * 10);
            Task resultTask = new Task(() => Console.WriteLine($"Результат выполнения задач: {task3.Result}"));

            Task[] tasks = new Task[]
            {
                task1, task2,task3,resultTask
            };

            foreach (Task tusk in tasks)
            {
                tusk.Start();
            }
        }

        public static  void Task4()
        {
            Task<int> task4 = Task.Run(() => Enumerable.Range(1, 100000).Count(x => (x % 2) == 0));
            task4.ContinueWith((x) => Console.WriteLine($"ContinueWith method running! Result :{task4.Result}"));

            var awaiter = task4.GetAwaiter();
            awaiter.OnCompleted(() => Console.WriteLine($"Awaiter running! Result : {awaiter.GetResult()}"));
        }

        public static void Task6()
        {
            Parallel.Invoke
                (
                () => Console.WriteLine(Enumerable.Range(1, 4000000).Count(x => (x % 2) == 0)), 
                () => Console.WriteLine(Enumerable.Range(1, 4000000).Count(x => (x % 2) == 1))
                );
            Console.ReadLine();
        }

        public static void Task5()
        {
            Parallel.For(1, 100000, x =>
            {
                if (IsPrime(x))
                {
                    Console.WriteLine(x);
                }

            });
        }



        private static bool IsPrime(int N)
        {
            for (int i = 2; i <= (int)(N / 2); i++)
            {
                if (N % i == 0)
                    return false;
            }
            return true;
        }

        static void PrimeNum()
        {

            int N = 500000;
            for (int i = 2; i <= N; i++)
            {
                if (IsPrime(i))
                {
                    //Console.Write(i.ToString() + ",");
                }

                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Операция прервана токеном");
                    return;
                }
            }
        }
    }
}
