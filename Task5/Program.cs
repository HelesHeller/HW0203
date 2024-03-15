using System.Threading;
using System;

namespace Task5
{
    internal class Program
    {
        static Semaphore semaphore = new Semaphore(3, 3);

        static void Main()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(DisplayRandomNumbers);
                thread.Start(i);
            }

            Console.ReadLine();
        }

        static void DisplayRandomNumbers(object threadId)
        {
            Console.WriteLine($"Поток {threadId} стартовал.");

            semaphore.WaitOne();

            try
            {
                Random random = new Random();
                for (int i = 0; i < 5; i++)
                {
                    int randomNumber = random.Next(1, 100);
                    Console.WriteLine($"Поток {threadId}: {randomNumber}");
                    Thread.Sleep(100);
                }
            }
            finally
            {
                semaphore.Release();
                Console.WriteLine($"Поток {threadId} завершил свою работу.");
            }
        }
    }
}