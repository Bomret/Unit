using System.Diagnostics;

namespace TheVoid {
    /// <summary>
    ///     Represents a type for the <see langword="void" /> keyword.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq")]
    public struct Unit {
        /// <summary>
        ///     Returns the default instance of <see cref="Unit"/>.
        /// </summary>
        public static Unit Default { get; } = default(Unit);

        /// <summary>
        ///     Returns the string representation of <see cref="Unit"/> which is "()".
        /// </summary>
        /// <returns>"()"</returns>
        public override string ToString() => "()";
    }
}