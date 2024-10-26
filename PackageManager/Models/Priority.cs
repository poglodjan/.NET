using System;

namespace PackageManager.Models
{
    [Flags]
    public enum Priority
    {
        Standard = 0,
        Express = 1,
        Fragile = 2
    }
}