using System;

namespace MiniTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeEachAttribute : Attribute { }
}