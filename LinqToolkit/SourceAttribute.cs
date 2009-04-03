using System;

namespace LinqToolkit {
    /// <summary>
    /// Defines the metadata attribute that specifies a data source name to read from.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct )]
    public class SourceAttribute: Attribute {
        public string Name { get; private set; }
        public SourceAttribute( string name ) {
            this.Name = name;
        }
    }
}
