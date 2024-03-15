using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;

namespace Task4
{
    internal class Program
    {
        static Mutex mutex = new Mutex(false, "UniqueMutex");

        static void Main()
        {
            Thread generatorThread = new Thread(GenerateAndSaveNumbers);
            generatorThread.Start();

            Thread analyzeThread = new Thread(AnalyzeNumbers);
            analyzeThread.Start();

            Thread analyzeWith7Thread = new Thread(AnalyzeNumbersWith7);
            analyzeWith7Thread.Start();

            generatorThread.Join();
            analyzeThread.Join();
            analyzeWith7Thread.Join();

            Console.WriteLine("Генерация, анализ и анализ с 7 завершены.");
        }

        static void GenerateAndSaveNumbers()
        {
            Console.WriteLine("Поток генерации: начало генерации и сохранения чисел.");
            mutex.WaitOne();

            Random random = new Random();
            using (StreamWriter writer = new StreamWriter("numbers.txt"))
            {
                for (int i = 0; i < 10; i++)
                {
                    int number1 = random.Next(1, 100);
                    int number2 = random.Next(1, 100);
                    writer.WriteLine($"{number1} {number2}");
                    Console.WriteLine($"Сохранена пара: {number1} {number2}");
                    Thread.Sleep(100);
                }
            }
            mutex.ReleaseMutex();
            Console.WriteLine("Поток генерации: завершение генерации.");
        }

        static void AnalyzeNumbers()
        {
            Console.WriteLine("Поток анализа: ожидание завершения генерации.");
            mutex.WaitOne();
            Console.WriteLine("Поток анализа: начало анализа.");

            string[] lines = File.ReadAllLines("numbers.txt");
            List<int> primes = new List<int>();

            foreach (var line in lines)
            {
                string[] numbers = line.Split(' ');
                int number1 = int.Parse(numbers[0]);
                int number2 = int.Parse(numbers[1]);

                if (IsPrime(number1))
                {
                    primes.Add(number1);
                }

                if (IsPrime(number2))
                {
                    primes.Add(number2);
                }
            }

            using (StreamWriter writer = new StreamWriter("primes.txt"))
            {
                foreach (var prime in primes)
                {
                    writer.WriteLine(prime);
                    Console.WriteLine($"Простое число: {prime}");
                    Thread.Sleep(100);
                }
            }
            mutex.ReleaseMutex();
            Console.WriteLine("Поток анализа: завершение анализа.");
        }

        static void AnalyzeNumbersWith7()
        {
            Console.WriteLine("Поток анализа с 7: ожидание завершения анализа.");
            mutex.WaitOne();
            Console.WriteLine("Поток анализа с 7: начало анализа.");

            string[] lines = File.ReadAllLines("primes.txt");
            List<int> primesWith7 = new List<int>();

            foreach (var line in lines)
            {
                int number = int.Parse(line);
                if (number % 10 == 7)
                {
                    primesWith7.Add(number);
                }
            }

            using (StreamWriter writer = new StreamWriter("primesWith7.txt"))
            {
                foreach (var prime in primesWith7)
                {
                    writer.WriteLine(prime);
                    Console.WriteLine($"Простое число с 7: {prime}");
                    Thread.Sleep(100);
                }
            }
            mutex.ReleaseMutex();
            Console.WriteLine("Поток анализа с 7: завершение анализа.");
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}
