namespace Task2
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    class Program
    {
        static int counter = 0;
        static object lockObject = new object();
        static Mutex mutex = new Mutex();
        static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            ExecuteTaskWithMonitor();
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения с использованием Monitor: {stopwatch.ElapsedMilliseconds} миллисекунд");

            counter = 0;
            stopwatch.Reset();
            stopwatch.Start();
            ExecuteTaskWithMutex();
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения с использованием Mutex: {stopwatch.ElapsedMilliseconds} миллисекунд");

            counter = 0;
            stopwatch.Reset();
            stopwatch.Start();
            ExecuteTaskWithLock();
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения с использованием lock: {stopwatch.ElapsedMilliseconds} миллисекунд");
        }

        static void ExecuteTaskWithMonitor()
        {
            Thread[] threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(IncrementCounterWithMonitor);
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        static void ExecuteTaskWithMutex()
        {
            Thread[] threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(IncrementCounterWithMutex);
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        static void ExecuteTaskWithLock()
        {
            Thread[] threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(IncrementCounterWithLock);
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        static void IncrementCounterWithMonitor()
        {
            for (int i = 0; i < 200000; i++)
            {
                Monitor.Enter(lockObject);
                try
                {
                    counter++;
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }

        static void IncrementCounterWithMutex()
        {
            for (int i = 0; i < 200000; i++)
            {
                mutex.WaitOne();
                try
                {
                    counter++;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        static void IncrementCounterWithLock()
        {
            for (int i = 0; i < 200000; i++)
            {
                lock (lockObject)
                {
                    counter++;
                }
            }
        }
    }
}
