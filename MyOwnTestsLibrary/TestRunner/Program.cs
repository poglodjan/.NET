using System;
using System.Reflection;
using System.Runtime.Loader;

namespace MiniTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("MiniTestRunner did not get the path in args, you can add them manually (C:\\Users\\...)");
                args = new[] { @"C:\Users\janpoglod\Desktop\C#\p1\MiniTest\AuthenticationService.Tests\bin\Debug\net6.0\AuthenticationService.Tests.dll" };
                // opcja recznego dodania pliku            
            }

            foreach (var assemblyPath in args)
            {
                try
                {
                    Console.WriteLine($"Loading assembly: {assemblyPath}");

                    var context = new AssemblyLoadContext(null, isCollectible: true);
                    var assembly = context.LoadFromAssemblyPath(assemblyPath);
                    // run tests with assembly
                    TestRunner.RunTests(assembly);
                    // free the memory
                    context.Unload();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex)
                {
                    ConsoleFormatter.WriteWarning($"Failed to load assembly: {ex.Message}");
                }
            }
        }
    }
}