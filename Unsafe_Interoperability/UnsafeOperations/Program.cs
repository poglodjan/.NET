// See https://aka.ms/new-console-template for more information
#define STAGE01
#define STAGE02
#define STAGE03

using System;
using System.Diagnostics;

namespace UnsafeWorkshop
{
    internal class Program
    {
        public static void MakeTitle(string title)
        {
            int totalWidth = 50;
            char decorationChar = '#';
            int titleLength = title.Length;
            int decorationLength = totalWidth - titleLength;

            int leftDecoration = decorationLength / 2;
            int rightDecoration = decorationLength - leftDecoration;

            Console.WriteLine($"\n\n{new string(decorationChar, leftDecoration)} {title} {new string(decorationChar, rightDecoration)}\n\n");
        }

        static void Main(string[] args)
        {



#if STAGE01
            {
                MakeTitle(" Unsafe sum");
                Random random = new Random(1);
                float[] randomNumbers = new float[2000];

                for (int i = 0; i < 1000; i++)
                {
                    randomNumbers[i] = (float)random.NextDouble();
                }

                Stopwatch sw = new Stopwatch();
                sw.Start();
                float sum = MathOperations.Sum(randomNumbers);
                sw.Stop();
                Console.WriteLine("Safe sum {0}, Elapsed={1}", sum, sw.Elapsed);

                sw.Restart();
                float sumUnsafe = MathOperations.Sum(randomNumbers);
                sw.Stop();
                Console.WriteLine("Unsafe sum {0}, Elapsed={1}", sumUnsafe, sw.Elapsed);
            }
#endif

#if STAGE02
            {
                unsafe
                {
                    MakeTitle(" Read bytes ");
                    int number = 1512126381;

                    // int* to byte*
                    int* intPointer = &number;
                    byte* bytePointer = (byte*)intPointer;

                    Console.WriteLine($"Integer: {number}");
                    Console.WriteLine("Bytes:");

                    for (int i = 0; i<sizeof(int); i++)
                    {
                        Console.WriteLine($"Byte {i}: {*(bytePointer + i):X2}");
                    }

                }
            }
#endif

#if STAGE03
            {
                MakeTitle(" UnsafeColor ");
                Span<UnsafeColor> unsafeColors = stackalloc UnsafeColor[10];
                Random random = new Random(1);

                unsafe
                {
                    float* colorComponents = stackalloc float[4];
                    for (int i = 0; i < 10; ++i)
                    {
                        for (int j = 0; j < 4; ++j)
                        {
                            colorComponents[j] = (float)random.NextDouble();
                        }
                        unsafeColors[i] = new UnsafeColor(colorComponents, 4); // fix is not needed
                    }

                    foreach (var color in unsafeColors)
                    {
                        Console.WriteLine(UnsafeColor.Negative(color * 0.5f));
                    }
                }
            }
#endif
        }
    }
}
