using System;
using System.Linq;
using System.Reflection;
using MiniTests.Attributes;

namespace MiniTestRunner
{
    public static class TestRunner
    {
        public static void RunTests(Assembly assembly)
        {
            var testClasses = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<TestClassAttribute>() != null);

            int totalTests = 0, passedTests = 0, failedTests = 0;

            foreach (var testClass in testClasses)
            {
                Console.WriteLine($"Running tests from class {testClass.FullName}...");

                if (testClass.GetConstructor(Type.EmptyTypes) == null)
                {
                    WriteInColor($"Skipping {testClass.Name}: No parameterless constructor.", ConsoleColor.Yellow);
                    continue;
                }

                var instance = Activator.CreateInstance(testClass);
                var beforeEach = BindDelegate(testClass, typeof(BeforeEachAttribute));
                var afterEach = BindDelegate(testClass, typeof(AfterEachAttribute));

                var testMethods = testClass.GetMethods()
                    .Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null)
                    .OrderBy(m => m.GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0)
                    .ThenBy(m => m.Name);

                int classTotal = 0, classPassed = 0, classFailed = 0;

                foreach (var testMethod in testMethods)
                {
                    var dataRows = testMethod.GetCustomAttributes<DataRowAttribute>().ToArray();
                    var description = testMethod.GetCustomAttribute<DescriptionAttribute>()?.Description;

                    if (dataRows.Length > 0)
                    {
                        foreach (var dataRow in dataRows)
                        {
                            classTotal++;
                            var result = ExecuteTest(instance, testMethod, beforeEach, afterEach, dataRow.Data, description);

                            PrintTestResult(testMethod.Name, result, dataRow.Description ?? string.Join(", ", dataRow.Data));
                            if (result) classPassed++; else classFailed++;
                        }
                    }
                    else
                    {
                        classTotal++;
                        var result = ExecuteTest(instance, testMethod, beforeEach, afterEach, null, description);

                        PrintTestResult(testMethod.Name, result, description);
                        if (result) classPassed++; else classFailed++;
                    }
                }

                Console.WriteLine(new string('*', 30));
                Console.WriteLine($"* Test passed:    {classPassed} / {classTotal}    *");
                Console.WriteLine($"* Failed:          {classFailed}         *");
                Console.WriteLine(new string('*', 30));
                Console.WriteLine(new string('#', 80));

                totalTests += classTotal;
                passedTests += classPassed;
                failedTests += classFailed;
            }

            Console.WriteLine("Summary of running tests:");
            Console.WriteLine(new string('*', 30));
            Console.WriteLine($"* Test passed:    {passedTests} / {totalTests}    *");
            Console.WriteLine($"* Failed:          {failedTests}         *");
            Console.WriteLine(new string('*', 30));
        }

        private static void PrintTestResult(string testName, bool result, string? description)
        {
            if (result)
            {
                WriteInColor($"{testName.PadRight(60)} : PASSED", ConsoleColor.Green);
            }
            else
            {
                WriteInColor($"{testName.PadRight(60)} : FAILED", ConsoleColor.Red);
                if (!string.IsNullOrEmpty(description))
                {
                    WriteInColor($"  {description}", ConsoleColor.Red);
                }
            }
        }

        private static bool ExecuteTest(object instance, MethodInfo testMethod,
            Func<object?, object?[]?, object?>? beforeEach,
            Func<object?, object?[]?, object?>? afterEach,
            object?[]? parameters, string? description)
        {
            try
            {
                beforeEach?.Invoke(instance, null);

                testMethod.Invoke(instance, parameters);

                return true;
            }
            catch (TargetInvocationException ex)
            {
                WriteInColor($"FAILED: {testMethod.Name} - {ex.InnerException?.Message}", ConsoleColor.Red);
                return false;
            }
            finally
            {
                afterEach?.Invoke(instance, null);
            }
        }

        private static Func<object?, object?[]?, object?>? BindDelegate(Type testClass, Type attributeType)
        {
            var method = testClass.GetMethods()
                .FirstOrDefault(m => m.GetCustomAttribute(attributeType) != null);

            return method == null ? null : (instance, args) => method.Invoke(instance, args);
        }

        private static void WriteInColor(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }
    }
}