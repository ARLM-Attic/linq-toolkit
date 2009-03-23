using System;
using System.Linq.Expressions;

namespace LinqToolkit.Test {

    public class TestBinaryOperation: IBaseOperation, IEquatable<TestBinaryOperation> {

        public readonly ExpressionType Type;
        public readonly string PropertyName;
        public readonly object Value;

        public TestBinaryOperation( ExpressionType type, string propertyName, object value ) {
            this.Type = type;
            this.PropertyName = propertyName;
            this.Value = value;
        }

        #region Equals support
        public bool Equals( TestBinaryOperation other ) {
            return
                this.Type.Equals( other.Type ) && 
                this.PropertyName.Equals( other.PropertyName ) &&
                this.Value.Equals( other.Value );
        }
        public override bool Equals( object obj ) {
            if ( obj is TestBinaryOperation ) {
                return this.Equals( (TestBinaryOperation)obj );
            }
            return base.Equals( obj );
        }
        public override int GetHashCode() {
            return
                this.Type.GetHashCode() ^
                this.PropertyName.GetHashCode() ^
                this.Value.GetHashCode();
        }
        #endregion Equals support
    }
}
