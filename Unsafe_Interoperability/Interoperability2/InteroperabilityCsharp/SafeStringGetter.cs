using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteroperabilityCsharp
{
    public class SafeStringGetter
    {
        public Action<IntPtr> FreeMemory;

        public SafeStringGetter(Action<IntPtr> freeMemory) { 
            FreeMemory = freeMemory;
        }

        public  string? GetStringAndFreeMemory(Func<IntPtr> CPPCallBack)
        {
            return null;
        }
    }
}
