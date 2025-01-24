using System;

namespace MiniTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; }
        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
}