using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            Task task = new Task(PrimeNum);
            task.Start();

            Console.WriteLine($"ID: { task.Id.ToString()}, Status: {task.Status}, Completed: {task.IsCompleted} ");


            

            

            string s = Console.ReadLine();

            if(s == "s")
            {
                cancelTokenSource.Cancel();
            }
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
