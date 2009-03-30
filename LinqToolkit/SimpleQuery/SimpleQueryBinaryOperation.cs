using System;
using System.Linq.Expressions;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryBinaryOperation: SimpleQueryBaseOperation {

        public ExpressionType Type { get; private set; }
        public string PropertyName { get; private set; }
        public object Value { get; private set; }

        internal SimpleQueryBinaryOperation( ExpressionType type, string propertyName, object value ) {
            this.Type = type;
            this.PropertyName = propertyName;
            this.Value = value;
        }
    }
}
