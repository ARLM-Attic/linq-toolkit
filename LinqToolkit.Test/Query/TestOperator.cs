using System;

namespace LinqToolkit.Test.Query {
    public class TestOperator: IEquatable<TestOperator> {

        public readonly string OperatorName;
        public readonly string PropertyName;
        public readonly object Value;

        public TestOperator( string operatorName ) {
            this.OperatorName = operatorName;
        }
        public TestOperator( string operatorName, string propertyName ) {
            this.OperatorName = operatorName;
            this.PropertyName = propertyName;
        }
        public TestOperator( string operatorName, object value ) {
            this.OperatorName = operatorName;
            this.Value = value;
        }

        #region Equals support
        public bool Equals( TestOperator other ) {
            return
                ( this.OperatorName==other.OperatorName ) &&
                ( this.PropertyName==other.PropertyName ) &&
                ( ( this.Value==null && other.Value==null ) || ( this.Value!=null && other.Value!=null && this.Value.Equals( other.Value ) ) );
        }
        public override bool Equals( object obj ) {
            if ( obj is TestOperator ) {
                return this.Equals( (TestOperator)obj );
            }
            return base.Equals( obj );
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        #endregion Equals support
    }
}
