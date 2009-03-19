using System;

namespace LinqToolkit {
    /// <summary>
    /// Defines an interface for join operation.
    /// </summary>
    public interface IJoinOperation: IBaseOperation {
        /// <summary>
        /// Left operand to join
        /// </summary>
        IBaseOperation Left { get; }
        /// <summary>
        /// Right operand to join
        /// </summary>
        IBaseOperation Right { get; }
    }
}
