using System.IO;
using System.Threading;
using System;

namespace Task1
{
    internal class Program
    {
        static AutoResetEvent generationEvent = new AutoResetEvent(false);
        static AutoResetEvent sumEvent = new AutoResetEvent(false);
        static AutoResetEvent productEvent = new AutoResetEvent(false);

        static void Main()
        {
            Thread generatorThread = new Thread(GenerateAndSaveNumbers);
            generatorThread.Start();

            Thread sumThread = new Thread(CalculateSum);
            sumThread.Start();

            Thread productThread = new Thread(CalculateProduct);
            productThread.Start();

            generatorThread.Join();
            sumThread.Join();
            productThread.Join();

            Console.WriteLine("Генерация, подсчет суммы и произведения завершены.");
        }

        static void GenerateAndSaveNumbers()
        {
            Console.WriteLine("Поток генерации: начало генерации и сохранения чисел.");

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
            generationEvent.Set();
            Console.WriteLine("Поток генерации: завершение генерации.");
        }

        static void CalculateSum()
        {
            Console.WriteLine("Поток суммы: ожидание завершения генерации.");
            generationEvent.WaitOne();
            Console.WriteLine("Поток суммы: начало подсчета суммы.");

            string[] lines = File.ReadAllLines("numbers.txt");
            using (StreamWriter writer = new StreamWriter("sums.txt"))
            {
                foreach (var line in lines)
                {
                    string[] numbers = line.Split(' ');
                    int sum = int.Parse(numbers[0]) + int.Parse(numbers[1]);
                    writer.WriteLine(sum);
                    Console.WriteLine($"Сумма: {sum}");
                    Thread.Sleep(100);
                }
            }
            sumEvent.Set();
            Console.WriteLine("Поток суммы: завершение подсчета суммы.");
        }

        static void CalculateProduct()
        {
            Console.WriteLine("Поток умножения: ожидание завершения суммации.");
            sumEvent.WaitOne();
            Console.WriteLine("Поток умножения: начало подсчета произведения.");

            string[] lines = File.ReadAllLines("numbers.txt");
            using (StreamWriter writer = new StreamWriter("products.txt"))
            {
                foreach (var line in lines)
                {
                    string[] numbers = line.Split(' ');
                    int product = int.Parse(numbers[0]) * int.Parse(numbers[1]);
                    writer.WriteLine(product);
                    Console.WriteLine($"Произведение: {product}");
                    Thread.Sleep(100);
                }
            }
            productEvent.Set();
            Console.WriteLine("Поток умножения: завершение подсчета произведения.");
        }
    }
}
