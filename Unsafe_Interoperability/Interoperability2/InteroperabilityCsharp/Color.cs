using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteroperabilityCsharp
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Color
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] rgba;

        public override string ToString()
        {
            return $"Name: {name}, RGBA: ({rgba[0]}, {rgba[1]}, {rgba[2]}, {rgba[3]})";
        }
    }
}
