using System;

namespace LinqToolkit {
    /// <summary>
    /// Defines the metadata attribute that specifies a property name to read from data source.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public class SourcePropertyAttribute: Attribute, IPropertyName {
        public string Name { get; private set; }
        public SourcePropertyAttribute( string name ) {
            this.Name = name;
        }
    }
}
