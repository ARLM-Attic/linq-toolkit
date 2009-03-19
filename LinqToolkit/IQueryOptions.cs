using System;
using System.Collections.Generic;

namespace LinqToolkit {
    /// <summary>
    /// Defines an interface for query options.
    /// </summary>
    public interface IQueryOptions {
        /// <summary>
        /// Filter based on query Where operation.
        /// </summary>
        IBaseOperation Filter { get; set; }
        /// <summary>
        /// Property names to read from data source.
        /// </summary>
        HashSet<string> PropertiesToRead { get; }
    }
}
