using System;

namespace Lab04.Models
{
    [Flags]
    public enum Priority
    {
        Standard = 0,
        Express = 1,
        Fragile = 2
    }
}