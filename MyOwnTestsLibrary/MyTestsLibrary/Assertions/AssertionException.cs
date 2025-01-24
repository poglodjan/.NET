using System;

namespace MiniTests.Assertions
{
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }
}