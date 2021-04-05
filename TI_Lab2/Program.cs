using System;
using System.Linq;

namespace TI_Lab2
{
    class Program
    {
        static bool IsTheNumberSimple(UInt64 Number)
        {
            if (Number < 2)
                return false;
            if (Number == 2)
                return true;
            for (UInt64 i = 2; i < Number; i++)
                if (Number % i == 0)
                    return false;
            return true;
        }

        static bool AreMutuallySimple(uint fi, uint e)
        {
            while (fi != e)
            {
                if (fi < e)
                    e = e - fi;
                else
                    fi = fi - e;
            }
            return (fi == 1) ? true : false;
        }

        static (long, long, long) Euclidex(uint a1, uint b1)
        {
            long a = (long)a1, b = (long)b1, x, y, d;
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

        static uint FastExp(uint a, uint z, uint n)
        {
            UInt64 a1 = (UInt64)a, z1 = (UInt64)z, x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = (UInt64)(z1 / 2);
                    a1 = (a1 * a1) % (UInt64)n;
                }
                z1 -= 1;
                x = (UInt64)(x * a1) % n;
            }
            return (uint)x;
        }

        static uint[] AlgorithmRSAEncrypt(uint e, uint r, string sourceText)
        {
            uint[] arr = new uint[sourceText.Length];
            for (int i = 0; i < sourceText.Length; i++)
                arr[i] = sourceText[i];
            uint[] cipherText = new uint[sourceText.Length];
            for (int i = 0; i < sourceText.Length; i++)
            {
                cipherText[i] = FastExp(arr[i], e, r);
            }

            return cipherText;
        }

        static string AlgorithmRSADecrypt(uint d, uint r, uint[] cipherText)
        {
            uint[] temp = new uint[cipherText.Length];
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

                uint p = 0, q = 0, e = 0;
                uint d = 0, r = 0, fi;
                string text = "", res = "";
                bool CheckingConditions = false;
                Random rnd = new Random();
                uint[] arr;
                bool isOK = true;

                if (act == 1)
                {
                    Console.WriteLine("Введите текст: ");
                    text += Console.ReadLine();
                    do
                    {
                        while (!CheckingConditions)
                        {
                            p = (uint)rnd.Next(10000, 100000);
                            CheckingConditions = IsTheNumberSimple(p);

                        }
                        CheckingConditions = false;
                        while (!CheckingConditions)
                        {
                            q = (uint)rnd.Next(10000, 100000);
                            CheckingConditions = IsTheNumberSimple(q);
                        }
                        r = p * q;
                        fi = (p - 1) * (q - 1);
                        CheckingConditions = false;
                        while (!CheckingConditions)
                        {
                            e = (uint)rnd.Next(600, 10000);
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
                        d = (uint)temp;

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

                    arr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => uint.Parse(i)).ToArray<uint>();
                    Console.WriteLine("Введите секретный ключ d: ");
                    d = (uint)Convert.ToUInt64(Console.ReadLine());
                    Console.WriteLine("Введите cекретный ключ r: ");
                    r = (uint)Convert.ToUInt64(Console.ReadLine());

                    res = AlgorithmRSADecrypt(d, r, arr);

                    Console.WriteLine("Расшифрованный текст: {0}", res);
                }
                Console.WriteLine("Нажмите ESC для выхода или другую клавишу для продолжения работы");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}