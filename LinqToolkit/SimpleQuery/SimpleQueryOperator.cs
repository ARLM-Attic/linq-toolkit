using System;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryOperator {

        public string OperatorName { get; private set; }
        public string PropertyName { get; private set; }
        public object Value { get; private set; }

        internal SimpleQueryOperator( string operatorName ) {
            this.OperatorName = operatorName;
        }
        internal SimpleQueryOperator( string operatorName, string propertyName )
            : this( operatorName ) {
            this.PropertyName = propertyName;
        }
        internal SimpleQueryOperator( string operatorName, object value )
            : this( operatorName ) {
            this.Value = value;
        }
    }
}
