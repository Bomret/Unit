using System.Diagnostics;

namespace TheVoid {
    /// <summary>
    ///     Represents a type for the <see langword="void" /> keyword.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq")]
    public struct Unit {
        public static Unit Default { get; } = default(Unit);

        public override string ToString() => "()";
    }
}