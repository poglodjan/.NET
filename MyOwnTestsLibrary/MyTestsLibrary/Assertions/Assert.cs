using System;

//
// Definition of assertions and ThrowsException implementation
//

namespace MiniTests.Assertions
{
    public static class Assert
    {
        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
                throw new AssertionException($"Assertion failed: {message}");
        }

        public static void IsFalse(bool condition, string message = "")
        {
            if (condition)
                throw new AssertionException($"Assertion failed: {message}");
        }

        public static void AreEqual<T>(T? expected, T? actual, string message = "")
        {
            if (!Equals(expected, actual))
                throw new AssertionException($"Expected: {expected}, but was: {actual}. {message}");
        }

        public static void AreNotEqual<T>(T? notExpected, T? actual, string message = "")
        {
            if (Equals(notExpected, actual))
                throw new AssertionException($"Did not expect: {notExpected}, but was: {actual}. {message}");
        }

        public static void ThrowsException<TException>(Action action, string message = "") where TException : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex) when (ex is TException)
            {
                return; 
            }
            catch (Exception ex)
            {
                throw new AssertionException($"Expected exception: {typeof(TException)}, but got: {ex.GetType()}. {message}");
            }

            throw new AssertionException($"Expected exception: {typeof(TException)}, but no exception was thrown. {message}");
        }

        public static void Fail(string message = "")
        {
            throw new AssertionException($"Test failed: {message}");
        }
    }
}