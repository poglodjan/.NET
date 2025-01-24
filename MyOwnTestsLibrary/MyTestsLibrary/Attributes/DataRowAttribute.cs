using System;

namespace MiniTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DataRowAttribute : Attribute
    {
        public object?[] Data { get; }
        public string Description { get; }

        // construct for description and data both
        public DataRowAttribute(string description, params object?[] data)
        {
            Description = description;
            Data = data;
        }
        //construct only for data
        public DataRowAttribute(params object?[] data)
        {
            Data = data;
        }
    }
}