using System;

namespace AMBehaviorSystem.Node.SourceGeneration
{
    [Flags]
    public enum ArgumentCategory
    {
        Integer = 1 << 0,
        Float = 1 << 1,
        Vector = 1 << 2,

        Scalar = Integer | Float,
        Numeric = Vector | Scalar,

        All = 1 << 3,
        None = 0
    }
}
