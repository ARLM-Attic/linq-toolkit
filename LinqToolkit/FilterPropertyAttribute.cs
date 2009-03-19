using System;

namespace LinqToolkit {
    /// <summary>
    /// Defines the metadata attribute that specifies a name which will be used to build an operation.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public class FilterPropertyAttribute: Attribute, IPropertyName {
        public string Name { get; private set; }
        public FilterPropertyAttribute( string name ) {
            this.Name = name;
        }
    }
}
