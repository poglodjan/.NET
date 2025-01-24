using System.Runtime.InteropServices;

namespace ManagedCode
{
    internal partial class Program
    {
        [DllImport("UnmanagedCode.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Hello();

        static void Main()
        {
            Hello();
        }
    }
}