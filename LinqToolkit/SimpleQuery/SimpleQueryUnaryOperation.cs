using System;
using System.Linq.Expressions;

namespace LinqToolkit.SimpleQuery {

    public class SimpleQueryUnaryOperation: SimpleQueryBaseOperation {

        public ExpressionType Type { get; private set; }
        public string PropertyName { get; private set; }

        internal SimpleQueryUnaryOperation( ExpressionType type, string propertyName ) {
            this.Type = type;
            this.PropertyName = propertyName;
        }
    }
}
