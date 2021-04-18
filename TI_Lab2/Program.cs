using System;
using System.Linq;
using System.Numerics;

namespace TI_Lab2
{
    class Program
    {
        static bool IsTheNumberSimple(long Number)
        {
            if (Number < 2)
                return false;
            if (Number == 2)
                return true;
            for (int i = 2; i < Math.Sqrt(Number) + 1; i++)
                if (Number % i == 0)
                    return false;
            return true;
        }

        static bool AreMutuallySimple(long fi, long e)
        {
            while (fi != e)
            {
                if (fi < e)
                    e = e - fi;
                else
                    fi = fi - e;
            }
            return (fi == 1);
        }

        static (long, long, long) Euclidex(long a1, long b1)
        {
            long a = a1, b = b1, x, y, d;
            long q, r, x1, x2, y1, y2;

            if (b == 0)
            {
                d = a;
                x = 1;
                y = 0;
                return (x, y, d);
            }

            x2 = 1;
            x1 = 0;
            y2 = 0;
            y1 = 1;

            while (b > 0)
            {
                q = a / b;
                r = a - q * b;
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b;
                b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            d = a;
            x = x2;
            y = y2;
            return (x, y, d);
        }

        static BigInteger FastExp(BigInteger a, BigInteger z, BigInteger n)
        {
            BigInteger a1 = a, z1 = z, x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % n;
                }
                z1 -= 1;
                x = (x * a1) % n;
            }
            return x;
        }

        static BigInteger[] AlgorithmRSAEncrypt(long e, long r, string sourceText)
        {
            long[] arr = new long[sourceText.Length];
            for (int i = 0; i < sourceText.Length; i++)
                arr[i] = sourceText[i];
            BigInteger[] cipherText = new BigInteger[sourceText.Length];
            for (int i = 0; i < sourceText.Length; i++)
            {
                cipherText[i] = FastExp(arr[i], e, r);
            }

            return cipherText;
        }

        static string AlgorithmRSADecrypt(long d, long r, BigInteger[] cipherText)
        {
            BigInteger[] temp = new BigInteger[cipherText.Length];
            string res = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                temp[i] = FastExp(cipherText[i], d, r);
            }
            for (int i = 0; i < cipherText.Length; i++)
            {
                res += (char)temp[i];
            }

            return res;
        }

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1. Зашифровать текст, 2. Расшифровать текст");
                int act = Convert.ToInt32(Console.ReadLine());

                long p = 0, q = 0, e = 0;
                long d = 0, r = 0, fi;
                string text = "", res = "";
                bool CheckingConditions = false;
                Random rnd = new Random();
                BigInteger[] arr;
                bool isOK = true;

                if (act == 1)
                {
                    Console.WriteLine("Введите текст: ");
                    text += Console.ReadLine();
                    do
                    {
                        while (!CheckingConditions)
                        {
                            p = rnd.Next(10000, 100000);
                            CheckingConditions = IsTheNumberSimple(p);

                        }
                        CheckingConditions = false;
                        while (!CheckingConditions)
                        {
                            q = rnd.Next(10000, 100000);
                            CheckingConditions = IsTheNumberSimple(q);
                        }
                        r = p * q;
                        fi = (p - 1) * (q - 1);
                        CheckingConditions = false;
                        while (!CheckingConditions)
                        {
                            e = rnd.Next(600, 10000);
                            CheckingConditions = AreMutuallySimple(fi, e);
                        }
                        var tuple = Euclidex(e, fi);
                        long temp = 0;
                        if (tuple.Item3 == 1)
                            temp = tuple.Item1;
                        else
                            isOK = false;
                        if (temp < 0)
                            temp += fi;
                        if (temp == 0)
                            isOK = false;
                        d = temp;

                    } while (!isOK);

                    Console.WriteLine("Открытый ключ: ({0}, {1})", e, r);
                    arr = AlgorithmRSAEncrypt(e, r, text);

                    Console.Write("Зашифрованный текст: { ");
                    for (int i = 0; i < arr.Length; i++)
                        Console.Write("{0} ", arr[i]);
                    Console.WriteLine("}");
                    Console.WriteLine("Секретный ключ: ({0}, {1})", d, r);
                }

                if (act == 2)
                {
                    Console.WriteLine("Введите текст: ");

                    arr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => BigInteger.Parse(i)).ToArray<BigInteger>();
                    Console.WriteLine("Введите секретный ключ d: ");
                    d = Convert.ToInt64(Console.ReadLine());
                    Console.WriteLine("Введите cекретный ключ r: ");
                    r = Convert.ToInt64(Console.ReadLine());

                    res = AlgorithmRSADecrypt(d, r, arr);

                    Console.WriteLine("Расшифрованный текст: {0}", res);
                }
                Console.WriteLine("Нажмите ESC для выхода или другую клавишу для продолжения работы");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}