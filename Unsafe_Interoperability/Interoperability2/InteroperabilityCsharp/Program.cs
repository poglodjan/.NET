#define STAGE01
#define STAGE02
#define STAGE03
#define STAGE04
#define STAGE05

using System.Reflection.Metadata;
using System;
using System.Runtime.InteropServices;
using System.Text;


namespace InteroperabilityCsharp
{
    public static partial class NativeLib
    {
        public delegate bool AreTheSameCommparer(string a, string b);
        const string unmanagedLibraryName = "InteroperabilityCPP";

#if STAGE01
        [DllImport(unmanagedLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SayFromCPP([MarshalAs(UnmanagedType.LPStr)] string mg);
#endif

#if STAGE02

        [DllImport(unmanagedLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetSecretMsgFromCPP();

        [DllImport(unmanagedLibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DestroySecretMsgFromCPP(IntPtr ptr);
        public string? GetStringAndFreeMemory(Func<IntPtr> CPPCallBack)
        {
            IntPtr ptr = CPPCallBack();
            if (ptr == IntPtr.Zero) return null;

            string result = Marshal.PtrToStringAnsi(ptr);
            FreeMemory(ptr); // call the function to free memory
            return result;
        }
#endif

#if STAGE03
        [DllImport(unmanagedLibraryName, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool AreTheSame(AreTheSameCommparer comparer, [MarshalAs(UnmanagedType.LPStr)] string a, [MarshalAs(UnmanagedType.LPStr)] string b);
#endif

#if STAGE04
        [LibraryImport(unmanagedLibraryName)]
        private static partial void SortNumbers(IntPtr arr, int size);
        public static void SortNumbers(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0) return;

            int size = numbers.Length;
            GCHandle handle = GCHandle.Alloc(numbers, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                SortNumbers(ptr, size); // Call the method
            }
            finally
            {
                handle.Free(); 
            }
        }

#endif

#if STAGE05
        [DllImport(unmanagedLibraryName)]
        internal static extern void Negative(ref Color color);
#endif
    }

    internal partial class Program
    {
        public static void MakeTitle(string title)
        {
            int totalWidth = 50;
            char decorationChar = '#';
            int titleLength = title.Length;
            int decorationLength = totalWidth - titleLength;

            int leftDecoration = decorationLength / 2;
            int rightDecoration = decorationLength - leftDecoration;

            Console.WriteLine( $"\n{new string(decorationChar, leftDecoration)} {title} {new string(decorationChar, rightDecoration)}\n");
        }

        static void Main(string[] args)
        {
#if STAGE01
            MakeTitle("Say hello");
            NativeLib.SayFromCPP("hello");
#endif

#if STAGE02
            MakeTitle("String from c++");
            SafeStringGetter safeExecutor = new SafeStringGetter(NativeLib.GetRidOfSecretMsg);
            string? notThatSecretMessage = safeExecutor.GetStringAndFreeMemory(NativeLib.SecretMsg);
            Console.WriteLine(notThatSecretMessage);
#endif

#if STAGE03
            MakeTitle("Compare strings");
            NativeLib.AreTheSameCommparer simpleComparer = delegate(string a, string b) { return a == b; };
            Console.WriteLine(simpleComparer( "to", "to"));
            Console.WriteLine(NativeLib.AreTheSame(simpleComparer, "to", "to"));
            Console.WriteLine(NativeLib.AreTheSame(simpleComparer, "to", "pho"));
#endif

#if STAGE04
            MakeTitle("Sort array");
            int[] numberToSort = new int[10] { 10, 2, 5, 1, 5, 1, 7, 8, 9, 10 };
            NativeLib.SortNumbers(numberToSort);

            Console.Write("Sorted array: ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(numberToSort[i] + ", ");
            }
#endif


#if STAGE05
            MakeTitle("Struct marshalling");
            Color color;
            { 
                color.name = "red";
                color.rgba = new float[4] { 1, 0, 0, 1 };
            }
            Console.WriteLine(color);
            NativeLib.Negative(ref color);
            Console.WriteLine(color);
#endif
        }
    }
}
