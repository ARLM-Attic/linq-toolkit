using System;

namespace LinqToolkit {
    /// <summary>
    /// Defines the metadata attribute to prevent property usage in queries.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public sealed class IgnoreAttribute: Attribute { }
}
