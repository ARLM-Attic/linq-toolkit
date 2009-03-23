using System;
using System.Linq;
using System.Reflection;

namespace LinqToolkit.Test {
    public class TestMethodCallOperation: IBaseOperation, IEquatable<TestMethodCallOperation> {

        public readonly MethodInfo Method;
        public readonly string PropertyName;
        public readonly object[] Arguments;

        public TestMethodCallOperation( MethodInfo method, string propertyName, object[] arguments ) {
            this.Method = method;
            this.PropertyName = propertyName;
            this.Arguments = arguments;
        }
        #region Equals support
        public bool Equals( TestMethodCallOperation other ) {
            return
                this.Method.Equals( other.Method ) && 
                this.PropertyName.Equals( other.PropertyName ) &&
                this.Arguments.Length==other.Arguments.Length &&
                !this.Arguments.Except( other.Arguments ).Any();
        }
        public override bool Equals( object obj ) {
            if ( obj is TestMethodCallOperation ) {
                return this.Equals( (TestMethodCallOperation)obj );
            }
            return base.Equals( obj );
        }
        public override int GetHashCode() {
            return
                this.Method.GetHashCode() ^
                this.PropertyName.GetHashCode();
        }
        #endregion Equals support
    }
}
