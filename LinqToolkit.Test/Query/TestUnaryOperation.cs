using System;
using System.Linq.Expressions;

namespace LinqToolkit.Test.Query {

    public class TestUnaryOperation: IBaseOperation, IEquatable<TestUnaryOperation> {

        public readonly ExpressionType Type;
        public readonly string PropertyName;

        public TestUnaryOperation( ExpressionType type, string propertyName ) {
            this.Type = type;
            this.PropertyName = propertyName;
        }
        #region Equals support
        public bool Equals( TestUnaryOperation other ) {
            return
                this.Type.Equals( other.Type ) &&
                this.PropertyName.Equals( other.PropertyName );
        }
        public override bool Equals( object obj ) {
            if ( obj is TestUnaryOperation ) {
                return this.Equals( (TestUnaryOperation)obj );
            }
            return base.Equals( obj );
        }
        public override int GetHashCode() {
            return
                this.Type.GetHashCode() ^
                this.PropertyName.GetHashCode();
        }
        #endregion Equals support
    }
}
